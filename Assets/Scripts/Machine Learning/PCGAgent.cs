using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.SceneManagement;

public class PCGAgent : Agent
{
    [SerializeField] TileStats tileStats;
    int reward;

    private void Start() {
        // Reward Events
        CastleTarget.OnSuccess += Success;
        Soldier.OnKilled += KilledSoldier;

        // Penalty Events
        CastleTarget.OnFail += Failure;
        TileHealth.OnTileDestroyed += TileDestroyed;
        Doorway.soldierWalkedThroughDoor += SoldierEnteredCastle;

        // As the entire scene is restarted each iteration, retrieve the saved reward value so the agent can continue where it left off
        reward = PlayerPrefs.GetInt("Reward", 0);
        SetReward(reward);
    }

    private void OnDestroy() {
        // Reward Events
        CastleTarget.OnSuccess -= Success;
        Soldier.OnKilled -= KilledSoldier;

        // Penalty Events
        CastleTarget.OnFail -= Failure;
        TileHealth.OnTileDestroyed -= TileDestroyed;
        Doorway.soldierWalkedThroughDoor -= SoldierEnteredCastle;
    }

    public override void OnEpisodeBegin() {

    }

    public override void CollectObservations(VectorSensor sensor) {
        // Add blank observations (observations aren't required for this specifix agent but this will prevent errors in the console)
        sensor.AddObservation(1);
        sensor.AddObservation(1);
        sensor.AddObservation(1);
        sensor.AddObservation(1);
        sensor.AddObservation(1);
    }

    public override void OnActionReceived(ActionBuffers actions) {
        // Adjust 'Standard' Tile Weight
        tileStats.AdjustDoorwayWeights(new int[] {
            actions.DiscreteActions[0],
            actions.DiscreteActions[1],
            actions.DiscreteActions[2],
            actions.DiscreteActions[3] });
        
        // Adjust Weapon Tile Weights
        tileStats.AdjustCannonWeights(new int[] { 
            actions.DiscreteActions[4], 
            actions.DiscreteActions[5], 
            actions.DiscreteActions[6], 
            actions.DiscreteActions[7] });
        tileStats.AdjustCatapultWeights(new int[] {
            actions.DiscreteActions[8],
            actions.DiscreteActions[9],
            actions.DiscreteActions[10],
            actions.DiscreteActions[11] });
        tileStats.AdjustCrossbowWeights(new int[] {
            actions.DiscreteActions[12],
            actions.DiscreteActions[13],
            actions.DiscreteActions[14],
            actions.DiscreteActions[15] });
        
        // Adjust Defense Tile Weight
        tileStats.AdjustStrongWallWeights(new int[] {
            actions.DiscreteActions[16],
            actions.DiscreteActions[17],
            actions.DiscreteActions[18],
            actions.DiscreteActions[19] });

    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        ActionSegment<int> continuousActions = actionsOut.DiscreteActions;

        // Set 'Standard' Tiles Base Stats
        continuousActions[0] = tileStats.BaseStats.baseDoorwayWeight;
        continuousActions[1] = tileStats.BaseStats.baseDoorwayWeight;
        continuousActions[2] = tileStats.BaseStats.baseDoorwayWeight;
        continuousActions[3] = tileStats.BaseStats.baseDoorwayWeight;

        // Set 'Weapon' Tiles Base Stats
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

        // Set 'Defense' Tiles Base Stats
        continuousActions[16] = tileStats.BaseStats.baseStrongWallWeight;
        continuousActions[17] = tileStats.BaseStats.baseStrongWallWeight;
        continuousActions[18] = tileStats.BaseStats.baseStrongWallWeight;
        continuousActions[19] = tileStats.BaseStats.baseStrongWallWeight;
    }

    private void KilledSoldier(GameObject soldier) {
        SetReward(+5f);
        reward += 5;
    }

    private void TileDestroyed(GameObject attacker) {
        // If the attacker is null, then return
        if(attacker == null) {
            return;
        }

        // Get the projectile component of the attacking object, and check if the object that instantiated it is a soldier
        CannonBall projectile = attacker.GetComponent<CannonBall>();
        Soldier soldier = null;
        if(projectile != null) {
            soldier = projectile.attacker.GetComponent<Soldier>();
        }

        // If the attacker is a soldier, then that means the destroyed object is a tile, so deal a penalty to the agent
        if (soldier != null) {
            SetReward(-1f);
            reward -= 1;
        }
    }

    private void SoldierEnteredCastle() {
        SetReward(-1f);
        reward -= 1;
    }

    private void Success() {
        // If the castle has wone, add a reward, save the current total reward value, and then reload the scene. 
        SetReward(+5f);
        reward += 5;
        EndEpisode();
        PlayerPrefs.SetInt("Reward", reward);
        SceneManager.LoadScene(0);
    }

    private void Failure() {
        // If the castle has failed, add a penalty, save the current total reward value, and then reload the scene. 
        SetReward(-1f);
        reward -= 1;
        EndEpisode();
        PlayerPrefs.SetInt("Reward", reward);
        SceneManager.LoadScene(0);
    }
}
