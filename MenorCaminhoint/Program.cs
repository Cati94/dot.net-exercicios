using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        var grafo = new Dictionary<string, Dictionary<string, int>>();

        Console.WriteLine("Digite as conexões do grafo no formato: De Para Distância");
        Console.WriteLine("Exemplo: A B 5");
        Console.WriteLine("Digite 'fim' para encerrar a entrada de conexões.");

        while (true)
        {
            Console.Write("Conexão: ");
            string linha = Console.ReadLine()!;
            if (linha.ToLower() == "fim") break;

            string[] partes = linha.Split(' ');
            if (partes.Length != 3)
            {
                Console.WriteLine("Entrada inválida. Tente novamente.");
                continue;
            }

            string de = partes[0];
            string para = partes[1];
            if (!int.TryParse(partes[2], out int distancia))
            {
                Console.WriteLine("Distância inválida. Digite um número.");
                continue;
            }

            if (!grafo.ContainsKey(de)) grafo[de] = new Dictionary<string, int>();
            grafo[de][para] = distancia;

            // garante que todos os nós existam no grafo
            if (!grafo.ContainsKey(para)) grafo[para] = new Dictionary<string, int>();
        }

        Console.Write("Digite a estação de origem: ");
        string origem = Console.ReadLine()!;

        Console.Write("Digite a estação de destino: ");
        string destino = Console.ReadLine()!;

        var (rota, distanciaTotal) = MenorCaminho(grafo, origem, destino);

        if (rota.Count == 0)
            Console.WriteLine("Não existe caminho entre as estações informadas.");
        else
            Console.WriteLine($"Rota: {string.Join(" -> ", rota)}, Distância: {distanciaTotal}");
    }

    static (List<string>, int) MenorCaminho(Dictionary<string, Dictionary<string, int>> grafo, string inicio, string destino)
    {
        var fila = new SortedSet<(int, string, List<string>)>(
            Comparer<(int, string, List<string>)>.Create((a, b) =>
            {
                int cmp = a.Item1.CompareTo(b.Item1);
                if (cmp == 0) cmp = a.Item2.CompareTo(b.Item2);
                return cmp;
            })
        );

        fila.Add((0, inicio, new List<string>()));
        var visitados = new HashSet<string>();

        while (fila.Count > 0)
        {
            var atual = fila.Min;
            fila.Remove(atual);

            int custo = atual.Item1;
            string no = atual.Item2;
            var caminho = new List<string>(atual.Item3) { no };

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

