public class QTable
{
    private float[,] table;
    private float learningRate;
    private float discountFactor;

    public QTable(int numStates, int numActions, float learningRate = 0.1f, float discountFactor = 0.9f)
    {
        table = new float[numStates, numActions];
        this.learningRate = learningRate;
        this.discountFactor = discountFactor;
    }

    public int GetNextAction(int state)
    {
        int numActions = table.GetLength(1);
        int bestAction = 0;
        float bestValue = table[state, 0];

        for (int action = 1; action < numActions; action++)
        {
            float value = table[state, action];
            if (value > bestValue)
            {
                bestValue = value;
                bestAction = action;
            }
        }

        return bestAction;
    }

    public void UpdateQValue(int state, int action, float reward)
    {
        int numActions = table.GetLength(1);
        float maxValue = table[state, 0];

        for (int a = 1; a < numActions; a++)
        {
            float value = table[state, a];
            if (value > maxValue)
            {
                maxValue = value;
            }
        }

        float currentValue = table[state, action];
        float newQValue = currentValue + learningRate * (reward + discountFactor * maxValue - currentValue);
        table[state, action] = newQValue;
    }
}