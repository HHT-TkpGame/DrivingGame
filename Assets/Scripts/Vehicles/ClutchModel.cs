using UnityEngine;

/// <summary>
/// �N���b�`�̌q�����i0�`1�j�ɉ����� Engine �� Transmission ��ڑ�
/// </summary>
public class ClutchModel
{
    VehicleInputHandler inputHandler;
    //�����ɓ���l��InputHandler����n�������͒l
    public float Engagement { get { return inputHandler.ClutchAxis; }  } //�N���b�`�̌q����
    public ClutchModel(VehicleInputHandler inputHandler)
    {
        this.inputHandler = inputHandler;
    }
}
