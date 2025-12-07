using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class AggroGroup : MonoBehaviour
{
    public class EntityAggro {
        public Entity entity;
        public int score;

        public EntityAggro(Entity entity) {
            this.entity = entity;
            this.score = 0;
        }
    }

    public class AggroList {
        private List<EntityAggro> l = new List<EntityAggro>();

        public EntityAggro SearchAggroList(Entity s) {
            foreach (EntityAggro a in l) {
                if (a.entity == s) return a;
            }

            return null;
        }

        public void Add(Entity s) {
            l.Add(new EntityAggro(s));
            l.Sort((EntityAggro x, EntityAggro y) => {
                if (x.score < y.score) return -1;
                else if (x.score > y.score) return 1;
                else return 1;
            });
        }

        public Entity Highest() {
            return l.Count > 0 ? l[0].entity : null;
        }
    }

    private AggroList aggroList = new AggroList();

    public UnityEvent<Entity> aggroEvent = new UnityEvent<Entity>();
    
    public Entity target { get { return aggroList.Highest(); } }

    public void UpdateAggroList(Entity source) {
        EntityAggro r;
        if ((r = aggroList.SearchAggroList(source)) != null) {
            r.score++;
        }
        else {
            aggroList.Add(source);
        }

        Debug.Log("Updating aggro list of " + gameObject.name + ": " + source.gameObject.name);

    }
}
