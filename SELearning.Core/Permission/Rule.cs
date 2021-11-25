namespace SELearning.Core.Permission;

public delegate Task<bool> Rule(object user); // TODO: Change object to an instance of user