using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

// Agentクラスの拡張クラス「RollerAgent」の定義
// エージェント：水色のボール
// ターゲット：黄色の箱
public class RollerAgent : Agent
{
    public Transform target; //ターゲット
    public int power;// エージェントに加える力の係数
    Rigidbody rBody; // エージェントのリジッドボディ

    // 初期化時に呼ばれる
    public override void Initialize()
    {
        this.rBody = GetComponent<Rigidbody>();
    }

    // 各エピソードにおける初期設定
    public override void OnEpisodeBegin()
    {
        // エージェントが床から落下したときの処理
        if (this.transform.localPosition.y < 0)
        {
            // エージェントの速度をリセット
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;

            // エージェントの位置をリセット
            this.transform.localPosition = new Vector3(0.0f, 0.5f, 0.0f);
        }

        // ターゲットをランダムな位置にリセット
        target.localPosition = new Vector3(
            Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
   
    }

    // 1. 状態取得　エージェントは環境を観察する（観察取得時に呼ばれるメソッド）
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(target.localPosition); // ターゲットの位置（XYZ座標）
        sensor.AddObservation(this.transform.localPosition); // エージェントの位置（XYZ座標）
        sensor.AddObservation(rBody.velocity.x); // エージェントのx方向の速度
        sensor.AddObservation(rBody.velocity.z); // エージェントのz方向の速度（上下運動はしないのでxとzのみを対象とする）
    }

    // 2. 行動決定


    // 3. 行動実行（上記2のポリシーにしたがって行動を実行）
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // 行動, size = 2（xとz方向への動きを対象とするので「2」）
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.z = actionBuffers.ContinuousActions[1];

        // エージェントに上記で設定した力を加える（x、z方向に動かす）
        rBody.AddForce(controlSignal * power);

        // エージェントとターゲットとの距離を測る
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, target.localPosition);

        // エージェントがターゲットに到達したとき（ぶつからないようにするために少し離れた位置を到達点として設定）
        if (distanceToTarget < 1.5f)
        {
            // 報酬を受け取りエピソードを終了する
            SetReward(1.0f);
            EndEpisode();
        }
        // エージェントが床から落下したとき
        else if (this.transform.localPosition.y < 0)
        {
            // エピソードを終了する
            EndEpisode();
        }
    }
    // ヒューリスティックモードの行動決定時に呼ばれる
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }
}
