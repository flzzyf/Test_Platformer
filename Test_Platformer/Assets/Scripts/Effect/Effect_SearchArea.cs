using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Effect_SearchArea")]
public class Effect_SearchArea : Effect
{
    public Vector2 offset;

    public float radius;

    public Effect effect;

    public Filter filter;

    public override void Trigger()
    {
        Vector2 newOffset = offset;
        newOffset.x *= caster.transform.localScale.x;
        Vector2 searchPos = (Vector2)caster.transform.position + newOffset;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(searchPos, radius);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].GetComponent<Flag>() != null)
            {
                bool pass = false;  //不合格
                for (int j = 0; j < filter.filterItems.Length; j++)
                {
                    if (colliders[i].GetComponent<Flag>().filterItems[j].Is != filter.filterItems[j].Is)
                    {
                        pass = true;
                        break;
                    }
                }

                if (!pass)
                {
                    //Debug.Log("目标物体" + colliders[i].name);

                    effect.Trigger(caster, colliders[i].gameObject);
                }


            }
        }
    }
}
