# Projeto N2 - IA

## Estrutura de dados:

**O projeto consiste, basicamente, em 5 scripts que organizam as cidades, as rotas, suas gerações e guardam essas informações para que o usuário possa analisar posteriormente.**


### CityManager.cs
O script CityManager é responsável por checar posições válidas para instanciar as cidades e as dispor de forma aleatória e também deletar as anteriores antes de criar um novo conjunto.

```c#
    public void SpawnCities(int citiesToSpawn) {
        //Limpa as cidades e depois cria novas, nos locais adequados.
    }

    public void DespawnCities() {
        //Limpas as cidades, assim como chama as funções de outros scripts que precisam ocorrer também para evitar desperdício de memória e garantir que o programa rode novamente sem erros.
    }

    private Vector3 GetRandomSpawnPosition() {
        //Retorna uma posição válida para criar uma cidade.
    }
```

### City.cs
O script City é um script simples que só contém um int público "cityId" utilizado por elementos da UI para dispor as rotas de uma forma mais legível ao usuário. Também é utilizado para que outros scripts possam identificar os GameObjects que sejam um cidade de forma mais eficiente.

### Route.cs
O script Route é um pouco mais complexo que os demais. Nele está a lógica que controla a formação das rotas individuais, que são guardadas dentro de uma lista de GameObjects. Dentro desse script foram necessários os seguintes métodos:

```c#

    private void Awake() {
        //Chama o método PopulateRoute(), para ter certeza que a rota inicialize com a lista completa de cidades.
    }

    public void PopulateRoute() {
        //Dentro deste método, o script popula a rota atrelada ao GameObject com uma rota de cidades, aleatoriza suas posições dentro da lista com o método ShuffleList(); e calcula a distancia total do percurso com o método CalculateTotalDistance();
    }

    private void ShuffleList() {
        //Cicla por todos os indexes da lista e substitui o GameObject ali presente por um aleatório, resultando em uma lista completamente reorganizada.
    }

    public void CalculateTotalDistance() {
        //Cicla por todas as cidades da rota em ordem calculando a distancia até o próximo ponto e adicionando o valor a variável totalDistance.
    }

    public void PopulateRouteCrossover(Route parentFirst, Route parentSecond) {
        //Este método recebe duas rotas como parametros, a melhor rota da geração e a segunda melhor e faz a cruza delas utilizando o método EmplaceSnippetAt(). Após esse processo, é calculada uma chance da rota ser mutada em 1, 3 ou 5 elementos, sendo estas mutações progressivamente mais raras e, por fim, calcula a distancia total desta nova rota.
    }

    private void Mutate(int amountToMutate) {
        //Este método é bem similar ao método ShuffleList(), mas este recebe um parametro de quantos elementos devem ser modificados, determinados no método PopulateRouteCorssOver(). Para cada elemento ele sorteia dois indexes na lista de cidades e os troca de posição. 
    }

    private void EmplaceSnippetAt(List<GameObject> snippet, int index) {
        //Este método recebe um "corte" de uma rota (retirado dos melhores da última geração) e sua posição original na lista e aloca este na nova rota, realocando elementos previamente dispostos.
    }

    private List<GameObject> GetBestSnippet(Route route) {
        //Este método é utilizado retornar o melhor corte de uma rota, ciclando por todos os trechos de rotas disponíveis e pegando aquele que tem a menor distancia total.
    }

    private float GetSnippetTotalDistance(List<GameObject> snippet) {
        //Utilizado pelo método GetBestSnippet() para calcular a distancia total de um corte.
    }

    private int CycleX(int i, int cycleLength) {
        //Método simples utilizado para ciclar pelas listas de forma que se um corte for adicionado ao fim de uma lista, ele continue sendo adicionado no inicio, ciclando até que o corte acabe.
    }

```

### Generation.cs
Nesse script é definido métodos para que cada geração seja populada com um número N de rotas, se elas serão cruzas de gerações anteriores e guardar as melhores rotas da geração.

```c#
    public void PopulateGen() {
        //Popula a geração sem fazer cruzamento de rotas.
    }

    public void PopulateGenCrossover() {
        //Popula a geração fazendo o cruzamento das duas melhores rotas da geração passada.
    }

    public void SetFittests() {
        //Calcula as melhores rotas da geração e salva elas nas váriaveis adequadas.
    }

    public void Clear() {
        //Limpa as rotas da geração.
    }

    public void SetFittestParents(Route parentFirst, Route parentSecond) {
        //Recebe parametros para setar quais são os "pais" desta geração.
    }

    public Route GetFit() => fittestRoute; //Retorna a melhor rota.
    public Route GetSecondFit() => secondFittestRoute; //Retorna a segunda melhor rota.
```

### TravellingManager.cs
O TravellingManager é o script que inicia os processos dos demais, sendo responsável por criar a primeira geração e "preparar o terreno" para as demais que serão crossovers da primeira. Também é responsável por limpar a lista de gerações e suas rotas e cidades uma vez que o usuário sorteie uma nova cidade, para evitar desperdício de memória.

```c#
    private void Update() {
        //Escuta alguns Inputs para Debug e controla o autoplay.
    }

    public void SetupFirst() {
        //Chama os métodos adequados à primeira geração.
    }

    public void NextGeneration() {
        //Chama os métodos adequados para criar todas as gerações subsequentes.
    }

    public void Reset() {
        //Limpa as informações do programa para evitar desperdício de memória ao criar novas cidades.
    }
```

## Fluxo do programa:
O ciclo do programa se dá inicio no TravellingManager que instancia a geração, chama o método para que ela seja populada e após isso calcule quais são as melhores rotas desta e as guarda. As rotas, por sua vez ao serem instanciadas se populam automáticamente com as cidades guardadas no CityManager, se aleatorizam e calculam a distancia total. Se esta não for a primeira geração, esse script acessa as informações da última geração e chama os métodos responsáveis para identificar e alocar os melhores cortes da última geração e após isso, sorteia para decidir se realiza uma mutação na rota cruzada.
