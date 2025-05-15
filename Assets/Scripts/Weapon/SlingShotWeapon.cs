using UnityEngine;

namespace Weapon
{
    public class SlingShotWeapon : ProjectileWeapon
    {
        [SerializeField] private float fireTime;
        [SerializeField] private float reloadTime;
        [SerializeField] private Transform fireLocation;
        [SerializeField] private GameObject fireArm;
        [SerializeField] private LeanTweenType fireEaseType;
        [SerializeField] private LeanTweenType reloadEaseType;
        protected override void Shoot(WeaponProjectile ammo)
        {
            var fireArmObject = fireArm;
            var fireArmLocation = fireArmObject.transform.position;
            ammo.transform.parent = fireArmObject.transform;
            LeanTween.move(fireArmObject.gameObject, fireLocation.position, fireTime).setEase(fireEaseType).setOnComplete(()=>
            {
                ammo.Shoot(GetClosestTarget());
                LeanTween.move(fireArmObject.gameObject, fireArmLocation, reloadTime).setEase(reloadEaseType).setOnComplete(ShootSequence);
            });
            
        }
    }
}
