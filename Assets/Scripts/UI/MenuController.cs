//加载指定页面关闭其他页面
using UnityEngine;
public class MenuController : MonoBehaviour
{
    public GameObject[] Menus;
    public void openMenu(GameObject targetMenu)
    {
        foreach(var menu in Menus)
        {
            menu.SetActive(menu == targetMenu);
        }
    }
    public void closeMenu()
    {
        foreach(var menu in Menus)
        {
            menu.SetActive(false);
        }
    }
     
}
