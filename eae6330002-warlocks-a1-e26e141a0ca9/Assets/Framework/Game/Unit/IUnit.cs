public interface IUnit {

    string Name { get; }
    IHealth Health { get; }

    Player Owner { get; }

}
