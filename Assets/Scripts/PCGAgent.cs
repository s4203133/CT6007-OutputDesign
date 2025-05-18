using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.SceneManagement;

public class PCGAgent : Agent
{
    [SerializeField] TileStats tileStats;

    private void Start() {
        Soldier.OnKilled += KilledSoldier;
        CastleTarget.OnSuccess += Success;
        CastleTarget.OnFail += Failure;
        TileHealth.OnTileDestroyed += TileDestroyed;
        Doorway.soldierWalkedThroughDoor += SoldierEnteredCastle;
    }

    private void OnDestroy() {
        Soldier.OnKilled -= KilledSoldier;
        CastleTarget.OnSuccess -= Success;
        CastleTarget.OnFail -= Failure;
        TileHealth.OnTileDestroyed -= TileDestroyed;
        Doorway.soldierWalkedThroughDoor -= SoldierEnteredCastle;
    }

    public override void OnEpisodeBegin() {

    }

    public override void CollectObservations(VectorSensor sensor) {

    }

    public override void OnActionReceived(ActionBuffers actions) {
        tileStats.AdjustDoorwayWeights(actions.DiscreteActions[0], actions.DiscreteActions[1], actions.DiscreteActions[2], actions.DiscreteActions[3]);
        
        tileStats.AdjustCannonWeights(actions.DiscreteActions[4], actions.DiscreteActions[5], actions.DiscreteActions[6], actions.DiscreteActions[7]);
        tileStats.AdjustCatapultWeights(actions.DiscreteActions[8], actions.DiscreteActions[9], actions.DiscreteActions[10], actions.DiscreteActions[11]);
        tileStats.AdjustCrossbowWeights(actions.DiscreteActions[12], actions.DiscreteActions[13], actions.DiscreteActions[14], actions.DiscreteActions[15]);

        tileStats.AdjustStrongWallWeights(actions.DiscreteActions[16], actions.DiscreteActions[17], actions.DiscreteActions[18], actions.DiscreteActions[19]);
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        ActionSegment<int> continuousActions = actionsOut.DiscreteActions;
        continuousActions[0] = tileStats.BaseStats.baseDoorwayWeight;
        continuousActions[1] = tileStats.BaseStats.baseDoorwayWeight;
        continuousActions[2] = tileStats.BaseStats.baseDoorwayWeight;
        continuousActions[3] = tileStats.BaseStats.baseDoorwayWeight;

        continuousActions[4] = tileStats.BaseStats.baseCannonWeight;
        continuousActions[5] = tileStats.BaseStats.baseCannonWeight;
        continuousActions[6] = tileStats.BaseStats.baseCannonWeight;
        continuousActions[7] = tileStats.BaseStats.baseCannonWeight;
        continuousActions[8] = tileStats.BaseStats.baseCatapultWeight;
        continuousActions[9] = tileStats.BaseStats.baseCatapultWeight;
        continuousActions[10] = tileStats.BaseStats.baseCatapultWeight;
        continuousActions[11] = tileStats.BaseStats.baseCatapultWeight;
        continuousActions[12] = tileStats.BaseStats.baseCrossbowWeight;
        continuousActions[13] = tileStats.BaseStats.baseCrossbowWeight;
        continuousActions[14] = tileStats.BaseStats.baseCrossbowWeight;
        continuousActions[15] = tileStats.BaseStats.baseCrossbowWeight;

        continuousActions[16] = tileStats.BaseStats.baseStrongWallWeight;
        continuousActions[17] = tileStats.BaseStats.baseStrongWallWeight;
        continuousActions[18] = tileStats.BaseStats.baseStrongWallWeight;
        continuousActions[19] = tileStats.BaseStats.baseStrongWallWeight;
    }

    private void KilledSoldier(GameObject soldier) {
        SetReward(+1f);
    }

    private void TileDestroyed(GameObject attacker) {
        if(attacker == null) {
            return;
        }
        CannonBall projectile = attacker.GetComponent<CannonBall>();
        Soldier soldier = null;

        if(projectile != null) {
            soldier = projectile.attacker.GetComponent<Soldier>();
        }

        if (soldier != null) {
            SetReward(-1f);
        }
    }

    private void SoldierEnteredCastle() {
        SetReward(-1f);
    }

    private void Success() {
        SetReward(+1f);
        EndEpisode();
        SceneManager.LoadScene(0);
    }

    private void Failure() {
        SetReward(-1f);
        EndEpisode();
        SceneManager.LoadScene(0);
    }
}
