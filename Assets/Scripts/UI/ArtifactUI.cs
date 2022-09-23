using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactUI : MonoBehaviour
{
    PlayerInfo playerInfo;
    [SerializeField]
    GameObject subContainer;
    List<List<Transform>> artifactSlotsList = new List<List<Transform>>();
    int curContainerIndex = 0;
    int curSlotIndex = 0;
    public void ArtifactDraw(Artifact artifact)
    {
        if(artifactSlotsList.Count == 0)
        {
            List<Transform> icons = new List<Transform>();
            for(int i =0; i<transform.childCount; i++)
            {
                icons.Add(transform.GetChild(i).transform);
            }
            artifactSlotsList.Add(icons);
        }
        if (curSlotIndex > 5)
        {
            curContainerIndex++;
            Transform sc = Instantiate(subContainer, transform.parent).transform;
            sc.localPosition += new Vector3(transform.GetComponent<RectTransform>().rect.width * curContainerIndex*0.8f, 0, 0);
            List<Transform> scSlot = new List<Transform>();
            for (int i = 0; i < 6; i++)
            {
                scSlot.Add(sc.GetChild(i));
            }
            artifactSlotsList.Add(scSlot);
            curSlotIndex = 0;
        }
        artifactSlotsList[curContainerIndex][curSlotIndex].GetComponent<Image>().sprite = artifact.simpleIcon;
        curSlotIndex++;
    }

}
