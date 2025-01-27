using UnityEngine;

public class CivilianBuildingsUI : ISingleton<CivilianBuildingsUI>
{
    private GameObject civilianBuildUI;
    [SerializeField] private Animator _animator;
    

    protected override void Awake()
    {
        civilianBuildUI = this.gameObject.transform.Find("CivilianBuildingsBuildUI").gameObject;
        _animator = GetComponent<Animator>();
    }

    public void OpenBuildUI(Vector2 position)
    {
        CameraScroll.Instance.canMoveCamera = false;
        civilianBuildUI.SetActive(true);
        _animator.SetTrigger("onEnable");
        civilianBuildUI.transform.position = Camera.main.WorldToScreenPoint(position);
    }

    public void CloseBuildUI()
    {
        if (civilianBuildUI.activeSelf)
        {
            _animator.SetTrigger("onDisable");
        }
            
    }

    public void DisableCivilianBuildUI()
    {
        //TODO KW: Hay que hacer un sistema para que ahora deje de clickar fuera y se pueda borrar, hemos creado el detectar si está en build ui
        civilianBuildUI.SetActive(false);
        CameraScroll.Instance.canMoveCamera = true;

    }


}