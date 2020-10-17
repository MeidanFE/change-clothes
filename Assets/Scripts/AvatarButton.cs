using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarButton : MonoBehaviour
{
    public void onValueChange(bool isOn) {
        if (isOn) {
            //if (gameObject.name == "boy" || gameObject.name == "girl") {
            //    sexChange(isOn);
            //    return;
            //}
            string[] names = gameObject.name.Split('-');
    
            AvatarSys._instance.onChangePeople(names[0],names[1]);
            string na = names[0].ToString();
            Debug.Log(na);
            switch (na) {
                case "pants":
                    PlayAnimation("item_pants");
                    break;
                case "shoes":
                    Debug.Log("shoes");
                    PlayAnimation("item_boots");
                    break;
                case "top":
                    PlayAnimation("item_shirt");
                    break;
                default:
                    break;
            }
        }
    }


    public void sexChange(bool isOn) {
        if (isOn)
        {

            AvatarSys._instance.SexChange();
        }
    }

    public void PlayAnimation(string animName)
    {
        GameObject player = GameObject.FindWithTag("Player");
        Animation anim = player.GetComponent<Animation>();
        Debug.Log(animName);
        if (!anim.IsPlaying(animName))
        {
            anim.Play(animName);
            anim.PlayQueued("idle1");
        }
    }

}
