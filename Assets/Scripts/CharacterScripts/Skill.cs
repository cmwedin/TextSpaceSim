public class Skill : Stat
{
    public Skill(string name) : base(name){
        Value = 10;
    }
    public Skill(string name, int val) : base(name, val) {}
    public override int Value 
        { get => base.Value; 
          set { 
                if(Value>100) {
                    UnityEngine.Debug.Log("Cannot set skills higher than 100");
                    Value = 100; }
              base.Value = value; }
        }
}