using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ForcedGravityRoom : Interactive
{
    public enum Orientation { south = 0, east = 1, north = 2, west = 3 }

    private Orientation _orientation;
    [SerializeField]
    private Orientation orientation;
    /*{
        get
        {
            return _orientation;
        }
        set
        {
            orientation = value;
            RotateParticles();
        }
    }*/
    private List<GravityShifter> shiftables = new List<GravityShifter>();
    private Transform particlesParent;
    private Transform particles;
    private float particleTop;
    private float particleWidth;
    private BoxCollider triggerBox;
    private ParticleSystem pSystem;

    // Start is called before the first frame update
    void Start()
    {
        triggerBox = GetComponent<BoxCollider>();
        particlesParent = transform.Find("Particles");
        particles = particlesParent.Find("Particle System");
        pSystem = GetComponentInChildren<ParticleSystem>();
        RotateParticles();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void RotateParticles()
    {
        particlesParent.rotation = Quaternion.Euler(Vector3.forward * ((int)orientation * 90));
        if ((int)orientation % 2 == 0)
        {
            particleTop = Mathf.Abs(triggerBox.bounds.extents.y);
            particleWidth = Mathf.Abs(triggerBox.bounds.extents.x);
        }
        else
        {
            particleTop = Mathf.Abs(triggerBox.bounds.extents.x);
            particleWidth = Mathf.Abs(triggerBox.bounds.extents.y);
        }
        particles.localPosition = Vector3.up * (particleTop - 0.5f);
        var main = pSystem.main;
        main.startLifetimeMultiplier = 0.37f * particleTop;
        var shape = pSystem.shape;
        shape.scale = Vector3.right * particleWidth * 2 + Vector3.up +Vector3.right;
    }

    public override void Interact(string input)
    {
        try
        {
            orientation = (Orientation)Enum.Parse(typeof(Orientation), input.ToLower());
            ShiftObjects();
        }
        catch
        {
            orientation = 0;
            ShiftObjects();
            Debug.Log("Couldn't change grav room properly");
        }
    }

    private void ShiftObjects()
    {
        RotateParticles();
        foreach (GravityShifter shiftable in shiftables)
        {
            shiftable.ForceGravity((int)orientation);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out GravityShifter shiftable))
        {
            shiftables.Add(shiftable);
        }
        ShiftObjects();
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out GravityShifter shiftable))
        {
            shiftables.Remove(shiftable);
            shiftable.Lock(false);
        }
    }
}
