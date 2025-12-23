using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Spellcasting : MonoBehaviour
{
    private LineRenderer spellLine;
    private LineRenderer hintLine;
    private Vector3 start;
    private bool drawing = false;
    private List<int> sections;
    private int[] hint;
    
    public float renderPlane = 5f;
    public Material lineMaterial;
    public float lineWidth = 0.2f;
    public Color startColor;
    public Color endColor;
    public GameObject gridDot;
    public float gridRadius = 1f;
    public float matchDistance = 0.1f;
    [SerializeField] public Color hintColor;

    void Start()
    {
        spellLine = gameObject.AddComponent<LineRenderer>();
        SetupLine(spellLine);

        hintLine = new GameObject("HintLine").AddComponent<LineRenderer>();
        hintLine.transform.SetParent(transform);
        SetupLine(hintLine);
        hintLine.startColor = hintColor;
        hintLine.endColor = hintColor;
        hintLine.startWidth = lineWidth / 2f;
        hintLine.endWidth = lineWidth / 2f;
    }

    private void SetupLine(LineRenderer lr)
    {
        lr.material = lineMaterial;
        lr.startColor = startColor;
        lr.endColor = endColor;
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
        lr.alignment = LineAlignment.TransformZ;
        lr.sortingLayerName = "Spellcasting";
    }

    void Update() {
        Vector3 mouse = Input.mousePosition;
        mouse.z = renderPlane;
        Vector3 target = Camera.main.ScreenToWorldPoint(mouse);

        if (Input.GetMouseButton(0)) {
            if (!drawing) {
                start = mouse;
                drawing = true;
                spellLine.enabled = true;
                sections = new List<int>();
                hint = GetHint();
                CreateGrid();
            }

            Vector3 worldStart = Camera.main.ScreenToWorldPoint(start);
            spellLine.positionCount = sections.Count + 2;
            spellLine.SetPosition(0, worldStart);
            Vector3 end = DetermineEndPosition();
            Vector3 currentCentre = worldStart;

            for (int i = 1; i <= sections.Count; i++) {
                currentCentre += GetGridPosition(sections[i-1]);
                spellLine.SetPosition(i, currentCentre);
            }

            if (hint == null || sections.Count >= hint.Length)
            {
                hintLine.enabled = false;
                hint = null;
            }
            else
            {
                hintLine.enabled = true;
                hintLine.SetPositions(new Vector3[] { currentCentre, currentCentre + GetGridPosition(hint[sections.Count]) });
            }

            spellLine.SetPosition(sections.Count + 1, target);

            Vector3 targetOffset = target - end;
            int p;
            if ((p = MatchToPoint(targetOffset)) != -1) {
                AudioManager.instance.connect.PlayAt(GameManager.instance.player.transform.position);
                if (hint != null && hint[sections.Count] != p) hint = null;
                sections.Add(p);
                CreateGrid();
            }
        }
        else if (drawing) {
            drawing = false;
            spellLine.enabled = false;
            hintLine.enabled = false;
            DestroyGrid();

            int[] pattern = sections.ToArray();
            Spell s = SpellEventManager.instance.DetermineSpell(pattern);

            if (s != null && GameManager.instance.player.mana >= s.manaCost) {
                SpellEventManager.instance.onSpellCast.Invoke(s, GameManager.instance.player.transform.position, s.DetermineSpellTarget(Camera.main.ScreenToWorldPoint(start), pattern, DetermineEndPosition()));
                GameManager.instance.player.StartAttackAnimation();
                GameManager.instance.player.AddMana(-s.manaCost);
            }
            else {
                SpellEventManager.instance.onSpellFailed.Invoke(pattern);
                AudioManager.instance.deny.PlayAt(GameManager.instance.player.transform.position);
            }
        }
    }

    private int[] GetHint()
    {
        foreach (SpellHint sd in FindObjectsByType<SpellHint>(FindObjectsSortMode.None))
            if (sd.inRange) return sd.hint;

        return null;
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
        int closest = -1;
        float cD = 1000;
        for (int i = 0; i < 6; i++) {
            float distance = (GetGridPosition(i) - centreOffset).magnitude;

            if (distance < matchDistance) {
                return i;
            }
            else if (distance <= cD) {
                closest = i;
                cD = distance;
            }
        }

        if (centreOffset.magnitude >= gridRadius) {
            return closest;
        }

        return -1;
    }
}
