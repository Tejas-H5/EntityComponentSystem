using System.Collections.Generic;
using System.Text;

namespace ECS
{
    public interface IECSSystem
    {
        void Update(float deltaTime);
    }
}
