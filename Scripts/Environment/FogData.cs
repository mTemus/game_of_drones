using UnityEngine;

public class FogData {
    private Color color;
    private float startDistance;
    private float endDistance;

    public FogData(Color color, float startDistance, float endDistance) {
        this.color = color;
        this.startDistance = startDistance;
        this.endDistance = endDistance;
    }

    public Color Color => color;

    public float StartDistance => startDistance;

    public float EndDistance => endDistance;
}
