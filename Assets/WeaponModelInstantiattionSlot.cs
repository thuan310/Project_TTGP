using UnityEngine;

public class WeaponModelInstantiattionSlot : MonoBehaviour
{
    // nhớ sửa lại model rồi chia từng phần tay chân với các bộ phận kĩ ra
    //Waht slot is this? ( left hand or right, or hips or back
    public WeaponModeSLot weaponSlot;
    public GameObject currentWeaponModel;

    public void UnloadWeapon()
    {
        if(currentWeaponModel != null)
        {
            Destroy(currentWeaponModel);
        }
    }    

    public void LoadWeapon(GameObject weaponModel)
    {
        currentWeaponModel = weaponModel;
        weaponModel.transform.parent = transform;

        weaponModel.transform.localPosition = Vector3.zero;
        weaponModel.transform.localRotation = Quaternion.identity;
        weaponModel.transform.localScale = Vector3.one;


    }
}
