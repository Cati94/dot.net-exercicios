using System;
using System.Collections.Generic;
using System.Linq;

/* Exercício: Menor caminho entre estações

Descrição:
Dado um grafo com estações e distâncias entre elas, encontre a rota mais curta entre uma estação de origem e uma estação de destino.

Exemplo de dados:

De Para	Distância
A	B	5
A	C	10
B	C	3
C	D	2
*/

class Program
{
    static void Main()
    {
        var grafo = new Dictionary<string, Dictionary<string, int>>()
        {
            {"A", new Dictionary<string,int>{{"B",5},{"C",10}}},
            {"B", new Dictionary<string,int>{{"C",3}}},
            {"C", new Dictionary<string,int>{{"D",2}}},
            {"D", new Dictionary<string,int>()}
        };

        var (rota, distancia) = MenorCaminho(grafo, "A", "D");
        Console.WriteLine($"Rota: {string.Join(" -> ", rota)}, Distância: {distancia}");
    }

    static (List<string>, int) MenorCaminho(Dictionary<string, Dictionary<string, int>> grafo, string inicio, string destino)
    {
        var fila = new SortedSet<(int, string, List<string>)>(
            Comparer<(int, string, List<string>)>.Create((a, b) =>
            {
                int cmp = a.Item1.CompareTo(b.Item1); // usa Item1 em vez de 'custo'
                if (cmp == 0) cmp = a.Item2.CompareTo(b.Item2); // Item2 em vez de 'no'
                return cmp;
            })
        );

        fila.Add((0, inicio, new List<string>()));
        var visitados = new HashSet<string>();

        while (fila.Count > 0)
        {
            var atual = fila.Min;
            fila.Remove(atual);

            int custo = atual.Item1;          // substitui atual.custo
            string no = atual.Item2;          // substitui atual.no
            var caminho = new List<string>(atual.Item3) { no }; // substitui atual.caminho

            if (no == destino)
                return (caminho, custo);

            if (visitados.Contains(no)) continue;
            visitados.Add(no);

            foreach (var vizinho in grafo[no])
            {
                if (!visitados.Contains(vizinho.Key))
                    fila.Add((custo + vizinho.Value, vizinho.Key, caminho));
            }
        }

        return (new List<string>(), int.MaxValue);
    }
}
 
