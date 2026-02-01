using UnityEngine;

public struct DirectionResolveResult
{
    public AnimationState state;
    public bool flipX;
}

public struct DirectionAnimationContext
{
    public Vector2 MoveDir;
    public Vector2 FacingDir;
}

public static class DirectionResolver
{
    public static DirectionResolveResult Resolve(DirectionAnimationContext ctx, DirectionalAnimationSet set)
    {
        Vector2 dir = ctx.MoveDir.sqrMagnitude > 0.01f ? ctx.MoveDir : ctx.FacingDir;

        DirectionResolveResult result = new DirectionResolveResult();

        // 1. Vertical dominates if available
        if (Mathf.Abs(dir.y) > Mathf.Abs(dir.x))
        {
            if (dir.y > 0 && set.up != null)
            {
                result.state = set.up;
                result.flipX = false;
                return result;
            }

            if (dir.y < 0 && set.down != null)
            {
                result.state = set.down;
                result.flipX = false;
                return result;
            }
        }

        // 2. Horizontal (Right or Left)
        if (dir.x > 0)
        {
            // Prefer right animation
            if (set.right != null)
            {
                result.state = set.right;
                result.flipX = false;
                return result;
            }

            // Fallback: use left animation flipped
            if (set.left != null)
            {
                result.state = set.left;
                result.flipX = true;
                return result;
            }
        }
        else
        {
            // Prefer left animation
            if (set.left != null)
            {
                result.state = set.left;
                result.flipX = false;
                return result;
            }

            // Fallback: use right animation flipped
            if (set.right != null)
            {
                result.state = set.right;
                result.flipX = true;
                return result;
            }
        }

        // 3. Final fallback: use ANY available animation
        if (set.down != null)
        {
            result.state = set.down;
            result.flipX = false;
            return result;
        }

        if (set.right != null)
        {
            result.state = set.right;
            result.flipX = false;
            return result;
        }

        if (set.up != null)
        {
            result.state = set.up;
            result.flipX = false;
            return result;
        }

        if (set.left != null)
        {
            result.state = set.left;
            result.flipX = false;
            return result;
        }

        // If literally nothing exists
        result.state = null;
        result.flipX = false;
        return result;
    }
}