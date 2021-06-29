[System.Serializable] public class Skill : Stat
{
    public Skill(string name) : base(name){
        Value = 10;
    }
    public Skill(string name, int val) : base(name, val) {}
    public Skill(string name, Attrib attrib, int threshold) : base(name) {
        Value = (int)(10 * attrib.Check(threshold)); 
    }
    public override int Value 
        { get => base.Value; 
          set { 
                if(Value>100) {
                    UnityEngine.Debug.Log("Cannot set skills higher than 100");
                    Value = 100; }
              base.Value = value; }
        }
}