using UnityEngine;

public interface ITransmissionModel
{
    /// <summary> 車輪側から見た最終のギア比（ギア * ファイナル） </summary>
    float CurrentRatio { get; }

    /// <summary> MT: -1,0,1.. / AT: -1,0,1(R,N,D) など、UI表示に使える </summary>
    int CurrentGear { get; }

    /// <summary> 入力（ギア/レンジ選択） </summary>
    void SetGear(int newGear);

    /// <summary> 速度上限などの更新が必要ならここで行う（MTは空実装でOK） </summary>
    void Tick(float speedKph, float deltaTime);

    /// <summary> 速度上限等で駆動トルクを絞りたい場合の係数（通常は1） </summary>
    float GetDriveTorqueScale(float speedKph);
}
