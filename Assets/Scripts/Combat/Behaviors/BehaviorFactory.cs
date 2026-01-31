using System.Collections.Generic;

public static class BehaviorFactory
{
    public static List<IBulletBehavior> Build(params BehaviorRequest[] requests)
    {
        List<IBulletBehavior> list = new List<IBulletBehavior>();

        foreach (var r in requests)
        {
            switch (r.Type)
            {
                case BehaviorType.Homing:
                    list.Add(new HomingBehavior(r.Target, r.FloatA));
                    break;

                case BehaviorType.SineWave:
                    list.Add(new SineWaveBehavior(r.Target.position - r.Spawner.transform.position, r.FloatA, r.FloatB));
                    break;

                case BehaviorType.Accelerate:
                    list.Add(new AcceleratingBehavior(r.FloatA));
                    break;

                case BehaviorType.Fade:
                    list.Add(new FadeBehavior(r.Renderer, r.FloatA));
                    break;

                case BehaviorType.Shrink:
                    list.Add(new ShrinkBehavior(r.FloatA));
                    break;

                case BehaviorType.Explode:
                    list.Add(new ExplodeBehavior(r.Spawner, r.FloatA, (int)r.FloatB, r.FloatC));
                    break;
            }
        }

        return list;
    }
}