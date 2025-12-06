using UnityEngine;
using System.Collections.Generic;

public class Spellcasting : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector3 start;
    private bool drawing = false;
    private List<int> sections;
    
    public float renderPlane = 5f;
    public Material lineMaterial;
    public float lineWidth = 0.2f;
    public Color startColor;
    public Color endColor;
    public GameObject gridDot;
    public float gridRadius = 1f;
    public float matchDistance = 0.1f;

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
    }

    void Update() {
        Vector3 mouse = Input.mousePosition;
        mouse.z = renderPlane;
        Vector3 target = Camera.main.ScreenToWorldPoint(mouse);

        if (Input.GetMouseButton(0)) {
            if (!drawing) {
                start = mouse;
                drawing = true;
                lineRenderer.enabled = true;
                sections = new List<int>();
                CreateGrid();
            }

            lineRenderer.startColor = startColor;
            lineRenderer.endColor = endColor;
            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;

            Vector3 worldStart = Camera.main.ScreenToWorldPoint(start);
            lineRenderer.positionCount = sections.Count + 2;
            lineRenderer.SetPosition(0, worldStart);
            Vector3 end = DetermineEndPosition();
            Vector3 currentCentre = worldStart;

            for (int i = 1; i <= sections.Count; i++) {
                currentCentre += GetGridPosition(sections[i-1]);
                lineRenderer.SetPosition(i, currentCentre);
            }

            lineRenderer.SetPosition(sections.Count + 1, target);

            Vector3 targetOffset = target - end;
            int p;
            if ((p = MatchToPoint(targetOffset)) != -1) {
                sections.Add(p);
                CreateGrid();
            }
        }
        else if (drawing) {
            drawing = false;
            lineRenderer.enabled = false;
            DestroyGrid();

            int[] pattern = sections.ToArray();
            Spell s = SpellEventManager.instance.DetermineSpell(pattern);

            if (s != null) {
                SpellEventManager.instance.onSpellCast.Invoke(s, GameManager.instance.player.transform.position, Camera.main.ScreenToWorldPoint(start));
            }
            else {
                SpellEventManager.instance.onSpellFailed.Invoke(pattern);
            }
        }
    }

    private Vector3 GetGridPosition(int index) {
        switch (index) {
            case 0: return new Vector3 (gridRadius / 2f, gridRadius * (Mathf.Sqrt(3f) / 2f), 0f);
            case 1: return new Vector3 (gridRadius, 0f, 0f);
            case 2: return new Vector3 (gridRadius / 2f, -gridRadius * (Mathf.Sqrt(3f) / 2f), 0f);
            case 3: return new Vector3 (-gridRadius / 2f, -gridRadius * (Mathf.Sqrt(3f) / 2f), 0f);
            case 4: return new Vector3 (-gridRadius, 0f, 0f);
            case 5: return new Vector3 (-gridRadius / 2f, gridRadius * (Mathf.Sqrt(3f) / 2f), 0f);
            default: return new Vector3();
        }
    }

    private Vector3 DetermineEndPosition() {
        Vector3 p = Camera.main.ScreenToWorldPoint(start);
        foreach (int i in sections) {
            p += GetGridPosition(i);
        }

        return p;
    }

    private void DestroyGrid() {
        foreach(GameObject o in GameObject.FindGameObjectsWithTag("CastingGrid")) Destroy(o);
    }

    private void CreateGrid() {
        DestroyGrid();

        Vector3 centre = DetermineEndPosition();

        for (int i = 0; i < 6; i++) {
            Transform t = Instantiate(gridDot, centre + GetGridPosition(i), new Quaternion()).transform;
            t.SetParent(transform);
        }
    }

    private int MatchToPoint(Vector3 centreOffset) {
        for (int i = 0; i < 6; i++) {
            float distance = (GetGridPosition(i) - centreOffset).magnitude;

            if (distance < matchDistance) {
                return i;
            }
        }

        return -1;
    }
}
