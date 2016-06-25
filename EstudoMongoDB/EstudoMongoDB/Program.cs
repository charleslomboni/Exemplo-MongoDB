using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace EstudoMongoDB {

    internal class Program {
        private static IMongoDatabase db;

        private static void Main(string[] args) {
            // Connection string na porta padrão do mongodb
            string connectionString = "mongodb://localhost:27017";

            MongoClient client = new MongoClient(connectionString);

            // Base de dados, se não existir antes do primeiro insert
            // É criada automaticamente
            db = client.GetDatabase("ExemploCSharp");

            // Cria a tabela de cidades de acordo com a classe Cidade
            var cidades = db.GetCollection<Cidade>("cidades");

            //var idRegistro = IncluirRegistro(cidades);
            //var wasUpdated = AtualizarRegistro(cidades);
            //var cidadesList = ListarTodos(cidades);
            //var itemEspecifico = ListarEspecifico("Peter Parker");
            var wasDeleted = Delete(cidades, "Peter Parker");
        }

        private static ObjectId IncluirRegistro(IMongoCollection<Cidade> cidades) {
            // Adiciona novo item
            Cidade cidade = new Cidade();
            cidade.Nome = "Charles Lomboni";
            cidade.Estado = "Rio de Janeiro";
            cidade.Pais = "Brasil";

            cidades.InsertOne(cidade);

            // Recupera ID da inclusão acima
            ObjectId _idInclusao = cidade._id;
            return _idInclusao;
        }

        private static bool AtualizarRegistro(IMongoCollection<Cidade> cidades) {
            // Encontra o registro
            Cidade atualizaCidade = cidades.Find(c => c.Nome == "Charles Lomboni").ToList().First();

            // Atualiza o valor do registro
            atualizaCidade.Estado = "RJ";

            // Atualiza no mongo
            ReplaceOneResult estadoReplace = cidades.ReplaceOne(c => c._id == atualizaCidade._id, atualizaCidade);

            // Se tiver encontrado e atualizado será maior que zero
            return estadoReplace.ModifiedCount > 0;
        }

        private static IEnumerable<Cidade> ListarTodos(IMongoCollection<Cidade> cidades) {
            // Listando todos os itens da lista
            List<Cidade> listCidades = db.GetCollection<Cidade>("cidades").Find(_ => true).ToList();
            return listCidades;
        }

        private static Cidade ListarEspecifico(string nome) {
            // Buscando item específico
            Cidade buscaCidade = db.GetCollection<Cidade>("cidades").Find(c => c.Nome == nome).ToList().First();
            return buscaCidade;
        }

        private static bool Delete(IMongoCollection<Cidade> cidades, string nome) {
            // Deletar documento
            DeleteResult objDelete = cidades.DeleteOne(c => c.Nome == nome);
            return objDelete.DeletedCount > 0;
        }
    }

    internal class Cidade {
        public ObjectId _id { get; set; }
        public string Nome { get; set; }
        public string Estado { get; set; }
        public string Pais { get; set; }
    }
}