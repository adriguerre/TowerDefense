using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;


namespace DependencyInjection
{
    /// <summary>
    /// Helper class that resolves dependencies in the Unity scene.
    /// </summary>
    public class DependencyResolver
{
    /// <summary>
    /// Enumerate the scene and find objects of interest to the dependency resolver.
    /// 
    /// WARNING: This function can be expensive. Call it only once and cache the result if you need it.
    /// </summary>
    public void FindObjects(IEnumerable<GameObject> allGameObjects, List<MonoBehaviour> injectables)
    {
        foreach (var gameObject in allGameObjects)
        {
            foreach (var component in gameObject.GetComponents<MonoBehaviour>())
            {
                var componentType = component.GetType();
                var hasInjectableProperties = componentType.GetProperties()
                    .Where(IsMemberInjectable)
                    .Any();
                if (hasInjectableProperties)
                {
                    injectables.Add(component);
                }
                else
                {
                    var hasInjectableFields = componentType.GetFields()
                        .Where(IsMemberInjectable)
                        .Any();
                    if (hasInjectableFields)
                    {
                        injectables.Add(component);
                    }
                }

                if (componentType.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(IsMemberInjectable)
                    .Any())
                {
                    Debug.LogError("Private properties should not be marked with [Inject] atttribute!", component);
                }

                if (componentType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(IsMemberInjectable)
                    .Any())
                {
                    Debug.LogError("Private fields should not be marked with [Inject] atttribute!", component);
                }
            }
        }
    }

    /// <summary>
    /// Determine if a member requires dependency resolution, checks if it has the 'Inject' attribute.
    /// </summary>
    private bool IsMemberInjectable(MemberInfo member)
    {
        return member.GetCustomAttributes(true)
            .Where(attribute => attribute is InjectAttribute)
            .Count() > 0;
    }

    /// <summary>
    /// Use C# reflection to find all members of an object that require dependency resolution and injection.
    /// </summary>
    private IEnumerable<IInjectableMember> FindInjectableMembers(MonoBehaviour injectable)
    {
        var type = injectable.GetType();
        var injectableProperties = type.GetProperties()
            .Where(IsMemberInjectable)
            .Select(property => new InjectableProperty(property))
            .Cast<IInjectableMember>();

        var injectableFields = type.GetFields()
            .Where(IsMemberInjectable)
            .Select(field => new InjectableField(field))
            .Cast<IInjectableMember>();

        return injectableProperties.Concat(injectableFields);
    }

    /// <summary>
    /// Get the ancestors from a particular Game Object. This means the parent object, the grand parent and so on up to the root of the hierarchy.
    /// </summary>
    private IEnumerable<GameObject> GetAncestors(GameObject fromGameObject)
    {
        for (var parent = fromGameObject.transform.parent; parent != null; parent = parent.parent)
        {
            yield return parent.gameObject; // Mmmmm... LINQ.
        }
    }

    /// <summary>
    /// Find matching dependencies at a particular level in the hiearchy.
    /// </summary>
    private IEnumerable<MonoBehaviour> FindMatchingDependendencies(Type injectionType, GameObject gameObject)
    {
        foreach (var component in gameObject.GetComponents<MonoBehaviour>())
        {
            if (injectionType.IsAssignableFrom(component.GetType()))
            {
                yield return component;
            }
        }
    }

    /// <summary>
    /// Find a single matching dependency at a particular level in the hierarchy.
    /// Returns null if none or multiple were found.
    /// </summary>
    private MonoBehaviour FindMatchingDependency(Type injectionType, GameObject gameObject, MonoBehaviour injectable)
    {
        var matchingDependencies = FindMatchingDependendencies(injectionType, gameObject).ToArray();
        if (matchingDependencies.Length == 1)
        {
            // A single matching dep was found.
            return matchingDependencies[0];
        }

        if (matchingDependencies.Length == 0)
        {
            // No deps were found.
            return null;
        }

        Debug.LogError(
            "Found multiple hierarchy dependencies that match injection type " + injectionType.Name + " to be injected into '" + injectable.name + "'. See following warnings.",
            injectable
        );

        foreach (var dependency in matchingDependencies)
        {
            Debug.LogWarning("  Duplicate dependencies: '" + dependency.name + "'.", dependency);
        }

        return null;
    }

    /// <summary>
    /// Walk up the hierarchy (towards the root) and find an injectable dependency that matches the specified type.
    /// </summary>
    private MonoBehaviour FindDependencyInHierarchy(Type injectionType, MonoBehaviour injectable)
    {
        foreach (var ancestor in GetAncestors(injectable.gameObject))
        {
            var dependency = FindMatchingDependency(injectionType, ancestor, injectable);
            if (dependency != null)
            {
                return dependency;
            }
        }

        return null;
    }

    /// <summary>
    /// Represents a member (property or field) of an object that have a dependency injected.
    /// </summary>
    public interface IInjectableMember
    {
        /// <summary>
        /// The one thing we want to do is set the value of the member.
        /// </summary>
        void SetValue(object owner, object value);

        /// <summary>
        /// Get the name  of the member.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Get the type of the member.
        /// </summary>
        Type MemberType { get; }

        /// <summary>
        /// The category of the member (field or property).
        /// </summary>
        string Category { get; }

        /// <summary>
        /// Specifies where the dependency is allowed to be injected from.
        /// </summary>
        InjectFrom InjectFrom { get; }
    }

    /// <summary>
    /// Represents a property of an object that have a dependency injected.
    /// </summary>
    public class InjectableProperty : IInjectableMember
    {
        private PropertyInfo propertyInfo;

        public InjectableProperty(PropertyInfo propertyInfo)
        {
            this.propertyInfo = propertyInfo;
            var injectAttribute = propertyInfo.GetCustomAttributes(typeof(InjectAttribute), false)
                .Cast<InjectAttribute>()
                .Single();
            this.InjectFrom = injectAttribute.InjectFrom;
        }

        /// <summary>
        /// The one thing we want to do is set the value of the member.
        /// </summary>
        public void SetValue(object owner, object value)
        {
            propertyInfo.SetValue(owner, value, null);
        }

        /// <summary>
        /// Get the name of the member.
        /// </summary>
        public string Name
        {
            get
            {
                return propertyInfo.Name;
            }
        }

        /// <summary>
        /// Get the type of the member.
        /// </summary>
        public Type MemberType
        {
            get
            {
                return propertyInfo.PropertyType;
            }            
        }

        /// <summary>
        /// The category of the member (field or property).
        /// </summary>
        public string Category
        {
            get
            {
                return "property";
            }
        }

        /// <summary>
        /// Specifies where the dependency is allowed to be injected from.
        /// </summary>
        public InjectFrom InjectFrom
        {
            get;
            private set;
        }
    }

    /// <summary>
    /// Represents a field of an object that have a dependency injected.
    /// </summary>
    public class InjectableField : IInjectableMember
    {
        private FieldInfo fieldInfo;

        public InjectableField(FieldInfo fieldInfo)
        {
            this.fieldInfo = fieldInfo;
            var injectAttribute = fieldInfo.GetCustomAttributes(typeof(InjectAttribute), false)
                .Cast<InjectAttribute>()
                .Single();
            this.InjectFrom = injectAttribute.InjectFrom;
        }

        /// <summary>
        /// The one thing we want to do is set the value of the member.
        /// </summary>
        public void SetValue(object owner, object value)
        {
            fieldInfo.SetValue(owner, value);
        }

        /// <summary>
        /// Get the name of the member.
        /// </summary>
        public string Name
        {
            get
            {
                return fieldInfo.Name;
            }
        }

        /// <summary>
        /// Get the type of the member.
        /// </summary>
        public Type MemberType
        {
            get
            {
                return fieldInfo.FieldType;
            }
        }

        /// <summary>
        /// The category of the member (field or property).
        /// </summary>
        public string Category
        {
            get
            {
                return "field";
            }
        }

        /// <summary>
        /// Specifies where the dependency is allowed to be injected from.
        /// </summary>
        public InjectFrom InjectFrom
        {
            get;
            private set;
        }
    }

    /// <summary>
    /// Attempt to resolve a member dependency by scanning up the hiearchy for a MonoBehaviour that mathces the injection type.
    /// </summary>
    private bool ResolveMemberDependencyFromHierarchy(MonoBehaviour injectable, IInjectableMember injectableMember)
    {
        // Find a match in the hierarchy.
        var toInject = FindDependencyInHierarchy(injectableMember.MemberType, injectable);
        if (toInject != null)
        {
            try
            {
                Debug.Log("Injecting " + toInject.GetType().Name + " from hierarchy (GameObject: '" + toInject.gameObject.name + "') into " + injectable.GetType().Name + " at " + injectableMember.Category + " " + injectableMember.Name + " on GameObject '" + injectable.name + "'.", injectable);

                injectableMember.SetValue(injectable, toInject);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex, injectable); // Bad type??!
            }

            return true;
        }
        else
        {
            // Failed to find a match.
            return false;
        }
    }
    
    /// <summary>
    /// Attempt to resolve a member dependency from anywhere in the scene.
    /// Returns false is no such dependency was found.
    /// </summary>
    private bool ResolveMemberDependencyFromAnywhere(MonoBehaviour injectable, IInjectableMember injectableMember)
    {
        if (injectableMember.MemberType.IsArray)
        {
            return ResolveArrayDependencyFromAnywhere(injectable, injectableMember);
        }
        else
        {
            return ResolveObjectDependencyFromAnywhere(injectable, injectableMember);
        }
    }

    /// <summary>
    /// Resolve an array dependency from objects anywhere in the scene.
    /// </summary>
    private static bool ResolveArrayDependencyFromAnywhere(MonoBehaviour injectable, IInjectableMember injectableMember)
    {
        var elementType = injectableMember.MemberType.GetElementType();
        var toInject = GameObject.FindObjectsOfType(elementType);
        if (toInject != null)
        {
            try
            {
                Debug.Log("Injecting array of " + toInject.Length + " elements into " + injectable.GetType().Name + " at " + injectableMember.Category + " " + injectableMember.Name + " on GameObject '" + injectable.name + "'.", injectable);

                foreach (var component in toInject.Cast<MonoBehaviour>())
                {
                    Debug.Log("> Injecting object " + component.GetType().Name + " (GameObject: '" + component.gameObject.name + "').", injectable);
                }

                // 
                // Create an appropriately typed array so that we don't get a type error when setting the value.
                //
                var typedArray = Array.CreateInstance(elementType, toInject.Length);
                Array.Copy(toInject, typedArray, toInject.Length);

                injectableMember.SetValue(injectable, typedArray);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex, injectable);
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Resolve an object dependency from objects anywhere in the scene.
    /// </summary>
    private static bool ResolveObjectDependencyFromAnywhere(MonoBehaviour injectable, IInjectableMember injectableMember)
    {
        var toInject = (MonoBehaviour)GameObject.FindObjectOfType(injectableMember.MemberType);
        if (toInject != null)
        {
            try
            {
                Debug.Log("Injecting object " + toInject.GetType().Name + " (GameObject: '" + toInject.gameObject.name + "') into " + injectable.GetType().Name + " at " + injectableMember.Category + " " + injectableMember.Name + " on GameObject '" + injectable.name + "'.", injectable);

                injectableMember.SetValue(injectable, toInject);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex, injectable);
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Resolve a member dependency and inject the resolved valued.
    /// </summary>
    private void ResolveMemberDependency(MonoBehaviour injectable, IInjectableMember injectableMember)
    {
        if (injectableMember.InjectFrom == InjectFrom.Above)
        {
            if (!ResolveMemberDependencyFromHierarchy(injectable, injectableMember))
            {
                Debug.LogError(
                    "Failed to resolve dependency for " + injectableMember.Category + ". Member: " + injectableMember.Name + ", MonoBehaviour: " + injectable.GetType().Name + ", GameObject: " + injectable.gameObject.name + "\r\n" +
                    "Failed to find a dependency that matches " + injectableMember.MemberType.Name + ".",
                    injectable
                );
            }
        }
        else if (injectableMember.InjectFrom == InjectFrom.Anywhere)
        {
            if (!ResolveMemberDependencyFromAnywhere(injectable, injectableMember))
            {
                Debug.LogError(
                    "Failed to resolve dependency for " + injectableMember.Category + ". Member: " + injectableMember.Name + ", MonoBehaviour: " + injectable.GetType().Name + ", GameObject: " + injectable.gameObject.name + "\r\n" +
                    "Failed to find a dependency that matches " + injectableMember.MemberType.Name + ".",
                    injectable
                );
            }
        }
        else
        {
            throw new ApplicationException("Unexpected use of InjectFrom enum: " + injectableMember.InjectFrom);
        }
    }

    /// <summary>
    /// Resolve dependenies for an 'injectable' object.
    /// </summary>
    private void ResolveDependencies(MonoBehaviour injectable)
    {
        var injectableProperties = FindInjectableMembers(injectable);
        foreach (var injectableMember in injectableProperties)
        {
            ResolveMemberDependency(injectable, injectableMember);
        }
    }

    /// <summary>
    /// Resolve all depenencies in the entire scene.
    /// </summary>
    public void ResolveScene()
    {
        var allGameObjects = GameObject.FindObjectsOfType<GameObject>();
        Resolve(allGameObjects);
    }

    /// <summary>
    /// Resolve a subset of the hierarchy downwards from a particular object.
    /// </summary>
    public void Resolve(GameObject parent)
    {
        var gameObjects = new GameObject[] { parent };
        Resolve(gameObjects);
    }

    /// <summary>
    /// Resolve dependencies for the hierarchy downwards from a set of game objects.
    /// </summary>
    public void Resolve(IEnumerable<GameObject> gameObjects)
    {
        var injectables = new List<MonoBehaviour>();
        FindObjects(gameObjects, injectables); // Scan the scene for objects of interest!

        foreach (var injectable in injectables)
        {
            ResolveDependencies(injectable);
        }
    }
}
}
