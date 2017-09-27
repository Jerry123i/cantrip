using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScriptGoblin : BasicRangedEnemyScript {

    public int multAtack;
	public int multAtackCounter=0;
	public float multAtacksInterval;

	public float offsetClock;
	public float offVectorX;
	public float offVectorY;
	public int turnerX = 1;
	public int turnerY = 1;
	public float tressholdX;
	public float tressholdY;
	Vector3 offsetVector = Vector3.zero;

	public override void Start() {
		base.Start();
		tressholdX = Random.Range(0.05f, 0.6f);
		tressholdY = Random.Range(0.05f, 0.6f);
	}

	public override void Update()
    {
        base.Update();
		OffsetVector();
    }


    public override void Shot()
    {
		base.Shot();
		multAtackCounter++;
		if (multAtackCounter <= multAtack - 1) {			
			clock = (1 / atkSpeed) / multAtacksInterval;
		} else {
			multAtackCounter = 0;
		}

    public override void RotateToPlayer()
    {        
        Vector3 direcao;
        float angle;
        direcao = player.transform.position + offsetVector - transform.position;
        angle = (Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg) - 90.0f;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * currentSpeed * 10.0f);

    }

	void OffsetVector() {

		offVectorX += Time.deltaTime * 3.0f * turnerX;
		offVectorY += Time.deltaTime * 3.0f * turnerY;

		if(offVectorX >= tressholdX && turnerX>0) {
			turnerX *= -1;
			tressholdX = Random.Range(0.05f, 0.6f) * turnerX;
		}
		if (offVectorX <= tressholdX && turnerX < 0) {
			turnerX *= -1;
			tressholdX = Random.Range(0.05f, 0.6f) * turnerX;
		}
		if (offVectorY >= tressholdY && turnerY > 0) {
			turnerY *= -1;
			tressholdY = Random.Range(0.05f, 0.6f) * turnerY;
		}
		if (offVectorY <= tressholdY && turnerY < 0) {
			turnerY *= -1;
			tressholdY = Random.Range(0.05f, 0.6f) * turnerY;
		}

		offsetVector.Set(offVectorX, offVectorY, 0.0f);

	}

}
