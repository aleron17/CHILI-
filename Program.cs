class Program
{
    public class DominoStones
    {
        public int First, Second;
        public DominoStones(int first, int second)
        {
            First = first;
            Second = second;
        }
        // Display the domino as [][]
        public override string ToString() => $"[{First}|{Second}]";
    }

    static void Main()
    {
        // Initialize the list of dominoes
        var dominoes = new List<DominoStones>
        {
            new DominoStones(2, 1),
            new DominoStones(2, 3),
            new DominoStones(1, 3)
        };
        // Find a circular chain of dominoes
        if (TryFindCircularChain(dominoes, out var circularChain))
        {
            Console.Write("Circular chain found: ");
            Console.WriteLine(string.Join(" ", circularChain));
        }
        else
            Console.WriteLine("No circular chain possible.");
    }

    // Tries to find a circular chain from the given list of dominoes
    static bool TryFindCircularChain(List<DominoStones> dominoes, out List<DominoStones> circularChain)
    {
        circularChain = new List<DominoStones>();
        if (dominoes.Count == 0)
            return false;

        // Find a valid chain starting with each domino
        for (int i = 0; i < dominoes.Count; i++)
        {
            var initialDomino = dominoes[i];
            var usedIndices = new HashSet<int> { i };
            circularChain.Add(initialDomino);

            if (FindChain(dominoes, circularChain, usedIndices, initialDomino.Second) &&
                circularChain.First().First == circularChain.Last().Second)
                return true;

            circularChain.Clear();
        }
        return false;
    }

    // Find a valid chain of dominoes
    static bool FindChain(List<DominoStones> dominoes, List<DominoStones> chain, HashSet<int> usedIndices, int match)
    {
        if (chain.Count == dominoes.Count)
            return true;

        for (int i = 0; i < dominoes.Count; i++)
        {
            if (usedIndices.Contains(i))
                continue; // Skip already used dominoes

            var domino = dominoes[i];

            // Check if the current domino can be connected to the chain
            if (domino.First == match || domino.Second == match)
            {
                usedIndices.Add(i); // Mark the domino as used, add the domino to the chain, flipping it if necessarys
                chain.Add(domino.First == match ? domino : new DominoStones(domino.Second, domino.First));
                // Recursively try to extend the chain
                if (FindChain(dominoes, chain, usedIndices, chain.Last().Second))
                    return true;

                // Remove the domino and unmark it as used
                chain.RemoveAt(chain.Count - 1);
                usedIndices.Remove(i);
            }
        }
        return false; // No valid chain found
    }
}
