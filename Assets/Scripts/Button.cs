using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    private MeshRenderer render;
    bool pressed = false;
    [SerializeField] private Material completedMat;
    Material[] m_Materials;

    void Start()
    {
        render = GetComponent<MeshRenderer>();
        m_Materials = render.materials;
    }
    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.tag == "Player" && pressed == false)
        {
            pressed = true;
            other.gameObject.GetComponent<PlayerMove>().buttonsPressed += 1;
            m_Materials[2] = completedMat;
            List<Material> mats = new List<Material>(m_Materials);
            render.SetMaterials(mats);

            Debug.Log("Button Pressed");
            // wait, then load next scene
        }
    }
}