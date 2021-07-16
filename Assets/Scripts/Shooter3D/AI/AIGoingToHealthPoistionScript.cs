using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class AIGoingToHealthPoistionScript : Agent
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer floorMeshRender;


    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-68f, -48f), -15, Random.Range(1f, 16f));
        targetTransform.localPosition = new Vector3(Random.Range(-35f, -15f), -15, Random.Range(128f, 142f));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        float moveX = vectorAction[0];
        float moveZ = vectorAction[1];

        float speed = 3f;
        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * speed;
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxisRaw("Horizontal");
        actionsOut[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<wall>(out wall wall)) {
            SetReward(-1f);
            floorMeshRender.material = loseMaterial;
            EndEpisode();
        }
        if (other.TryGetComponent<BossController>(out BossController bossController))
        {
            SetReward(+1f);
            floorMeshRender.material = winMaterial;
            EndEpisode();
        }
    }
}
