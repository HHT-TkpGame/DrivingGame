
using UnityEngine;
/// <summary>
/// �N���b�`�̌q�����i0�`1�j�ɉ����� Engine �� Transmission ��ڑ�
/// </summary>
public class TmpClutchModel
{
    VehicleInputHandler inputHandler;
    AnimationCurve clutchCurve;
    //�N���b�`�̐ڑ����(1���ڑ�, 0���ؒf)
    //InputHandler����̓��͉͂�����Ă��Ȃ�����0��Ԃ�
    //�{���͓��͂��Ȃ����ɐڑ���Ԃ�1�ɂȂ�ׂ��Ȃ̂Œl�𔽑΂ɂ���
    public float Engagement { 
        get 
        {
            float raw = 1 - inputHandler.ClutchAxis;
            Debug.Log(raw);
            return clutchCurve.Evaluate(raw); 
        }
    } 
    public TmpClutchModel(VehicleInputHandler inputHandler, AnimationCurve clutchCurve)
    {
        this.inputHandler = inputHandler;
        this.clutchCurve = clutchCurve;
    }
}
