using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

// Agentクラスの拡張クラス「RollerAgent」の定義
// エージェント：水色のボール
// ターゲット：黄色の箱
public class RaycastAgent : Agent
{
    public Transform target; //ターゲット
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


    // 行動実行（上記2のポリシーにしたがって行動を実行）
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // 行動, size = 2（xとz方向への動きを対象とするので「2」）
        Vector3 dirToGo = Vector3.zero;
        Vector3 rotateDir = Vector3.zero;

        int action = (int)actionBuffers.DiscreteActions[0];

        if (action == 1) dirToGo = transform.forward;
        if (action == 2) dirToGo = transform.forward * -1.0f;
        if (action == 3) rotateDir = transform.up * -1.0f;
        if (action == 4) rotateDir = transform.up;

        this.transform.Rotate(Time.deltaTime * 200f * rotateDir);

        // エージェントに上記で設定した力を加える（x、z方向に動かす）
        this.rBody.AddForce(dirToGo * 0.4f, ForceMode.VelocityChange);

        // エージェントとターゲットとの距離を測る
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, target.localPosition);

        // エージェントがターゲットに到達したとき（ぶつからないようにするために少し離れた位置を到達点として設定）
        if (distanceToTarget < 1.5f)
        {
            // 報酬を受け取りエピソードを終了する
            AddReward(1.0f);
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
        ActionSegment<int> actions = actionsOut.DiscreteActions;

        actions[0] = 0;
        if (Input.GetKey(KeyCode.UpArrow)) actions[0] = 1;
        if (Input.GetKey(KeyCode.DownArrow)) actions[0] = 2;
        if (Input.GetKey(KeyCode.LeftArrow)) actions[0] = 3;
        if (Input.GetKey(KeyCode.RightArrow)) actions[0] = 4;
    }
}
