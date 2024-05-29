using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading;

namespace SafeEats
{
    internal class Program
    {
        public class TelaLogin
        {
            string emailLogado;
            public void menuInicial()
            {
                string conString = @"Data Source=DESKTOP-UUDL5AC\SQLEXPRESS;Initial Catalog=SafeEats;Integrated Security=True;";

                bool sairLoop = false;
                while (!sairLoop)
                {
                    Console.WriteLine("\t\t▄██▄▄██▄ ▄█████ ██████▄ ██   ██   ██████ ██████▄ ██████ ▄█████ ██████ ▄████▄ ██    \r\n" +
                                      "\t\t██ ██ ██ ██     ██   ██ ██   ██     ██   ██   ██   ██   ██       ██   ██  ██ ██    \r\n" +
                                      "\t\t██ ██ ██ █████  ██   ██ ██   ██     ██   ██   ██   ██   ██       ██   ██  ██ ██    \r\n" +
                                      "\t\t██ ██ ██ ██     ██   ██ ██   ██     ██   ██   ██   ██   ██       ██   ██████ ██    \r\n" +
                                      "\t\t██ ██ ██ ▀█████ ██   ██ ▀█████▀   ██████ ██   ██ ██████ ▀█████ ██████ ██  ██ ██████\r\n");
                    Console.Write("F1 - Fazer Login\nF2 - Criar uma conta");

                    ConsoleKeyInfo keyPressed = Console.ReadKey(true);
                    switch (keyPressed.Key)
                    {
                        case ConsoleKey.F1:
                            login();
                            break;
                        case ConsoleKey.F2:
                            Console.Clear();
                            cadastrarUsuario();
                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine("Opção inválida! =(");
                            Thread.Sleep(1000);
                            Console.Clear();
                            break;
                    }
                }

                void mascaraSenha(ref string senha)
                {
                    ConsoleKeyInfo key;
                    do
                    {
                        key = Console.ReadKey(true);
                        if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                        {
                            senha += key.KeyChar;
                            Console.Write("*");
                        }
                        else if (key.Key == ConsoleKey.Backspace && senha.Length > 0)
                        {
                            senha = senha.Substring(0, senha.Length - 1);
                            Console.Write("\b \b");
                        }
                    } while (key.Key != ConsoleKey.Enter);

                    Console.WriteLine(); // Nova linha após a senha
                }

                void mensagemCadastro()
                {
                    Console.WriteLine("\t\t\t▄█████ ▄████▄ ██████▄ ▄████▄ ▄██████ ████████ █████▄ ▄█████▄\r\n" +
                                      "\t\t\t██     ██  ██ ██   ██ ██  ██ ██         ██    ██  ██ ██   ██\r\n" +
                                      "\t\t\t██     ██  ██ ██   ██ ██  ██ ▀█████▄    ██    █████▀ ██   ██\r\n" +
                                      "\t\t\t██     ██████ ██   ██ ██████      ██    ██    ██  ██ ██   ██\r\n" +
                                      "\t\t\t▀█████ ██  ██ ██████▀ ██  ██ ██████▀    ██    ██  ██ ▀█████▀\n");
                }

                void mensagemLogin()
                {
                    Console.WriteLine("\t\t\t\t██     ▄█████▄ ▄██████ ██████ ██████▄\r\n" +
                                      "\t\t\t\t██     ██   ██ ██        ██   ██   ██\r\n" +
                                      "\t\t\t\t██     ██   ██ ██  ███   ██   ██   ██\r\n" +
                                      "\t\t\t\t██     ██   ██ ██   ██   ██   ██   ██\r\n" +
                                      "\t\t\t\t██████ ▀█████▀ ▀█████▀ ██████ ██   ██\r\n");
                }

                void cadastrarUsuario()
                {
                    string nomeCompleto, cpf, email, telefone, senhaUsuario, confirmarSenha, rua, numero, complemento, cep;

                    using (SqlConnection conexao = new SqlConnection(conString))
                    {
                        conexao.Open();
                        mensagemCadastro();

                        while (true)
                        {
                            Console.Write("Digite o nome completo: ");
                            nomeCompleto = Console.ReadLine();

                            if (string.IsNullOrWhiteSpace(nomeCompleto))
                            {
                                Console.Clear();
                                mensagemCadastro();
                                Console.WriteLine("O nome não pode estar vazio. Por favor, digite novamente.");
                                Thread.Sleep(1500);
                                Console.Clear();
                                mensagemCadastro();
                                continue; // Retorna ao início do loop para solicitar o nome novamente
                            }
                            else
                            {
                                break; // Sai do loop se o nome não estiver vazio
                            }
                        }

                        while (true)
                        {
                            Console.Write("Digite o CPF: ");
                            cpf = Console.ReadLine();

                            if (string.IsNullOrWhiteSpace(cpf))
                            {
                                Console.Clear();
                                mensagemCadastro();
                                Console.WriteLine("O CPF não pode estar vazio. Por favor, digite novamente.");
                                Thread.Sleep(1500);
                                Console.Clear();
                                mensagemCadastro();
                                Console.WriteLine($"Nome completo: {nomeCompleto}");
                                continue; // Retorna ao início do loop para solicitar o nome novamente
                            }
                            else
                            {
                                break; // Sai do loop se o nome não estiver vazio
                            }
                        }

                        while (true)
                        {
                            Console.Write("Digite o e-mail: ");
                            email = Console.ReadLine();

                            if (string.IsNullOrWhiteSpace(email))
                            {
                                Console.Clear();
                                mensagemCadastro();
                                Console.WriteLine("O e-mail não pode estar vazio. Por favor, digite novamente.");
                                Thread.Sleep(1500);
                                Console.Clear();
                                mensagemCadastro();
                                Console.WriteLine($"Nome completo: {nomeCompleto}\nCPF: {cpf}");
                                continue; // Retorna ao início do loop para solicitar o e-mail novamente
                            }

                            // Consulta SQL para verificar se o e-mail já existe
                            string verificarEmailSql = "SELECT COUNT(*) FROM Usuario WHERE email = @Email";
                            SqlCommand cmdVerificarEmail = new SqlCommand(verificarEmailSql, conexao);
                            cmdVerificarEmail.Parameters.AddWithValue("@Email", email);
                            int count = (int)cmdVerificarEmail.ExecuteScalar();

                            // Verifica se o e-mail já existe
                            if (count > 0)
                            {
                                Console.Clear();
                                mensagemCadastro();
                                Console.WriteLine("O e-mail já está em uso! Por favor, escolha outro.");
                                Thread.Sleep(1500);
                                Console.Clear();
                                mensagemCadastro();
                                Console.WriteLine($"Nome completo: {nomeCompleto}\nCPF: {cpf}");
                                continue; // Retorna ao início do loop para solicitar o e-mail novamente
                            }
                            else
                            {
                                break; // Sai do loop se o e-mail não existir
                            }
                        }

                        while (true)
                        {
                            Console.Write("Digite o telefone: ");
                            telefone = Console.ReadLine();

                            if (string.IsNullOrWhiteSpace(telefone))
                            {
                                Console.Clear();
                                mensagemCadastro();
                                Console.WriteLine("O telefone não pode estar vazio. Por favor, digite novamente.");
                                Thread.Sleep(1500);
                                Console.Clear();
                                mensagemCadastro();
                                Console.WriteLine($"Nome completo: {nomeCompleto}\nCPF: {cpf}\nE-mail: {email}");
                                continue;
                            }
                            else
                            {
                                break; // Sai do loop se o nome não estiver vazio
                            }
                        }

                        Console.Clear();
                        mensagemCadastro();

                        while (true)
                        {
                            Console.Write("Digite o nome da rua: ");
                            rua = Console.ReadLine();

                            if (string.IsNullOrWhiteSpace(rua))
                            {
                                Console.Clear();
                                mensagemCadastro();
                                Console.WriteLine("O nome da rua não pode estar vazio. Por favor, digite novamente.");
                                Thread.Sleep(1500);
                                Console.Clear();
                                mensagemCadastro();
                                continue; // Retorna ao início do loop para solicitar o nome da rua novamente
                            }
                            else
                            {
                                break; // Sai do loop se o nome da rua não estiver vazio
                            }
                        }

                        while (true)
                        {
                            Console.Write("Digite o número: ");
                            numero = Console.ReadLine();

                            if (string.IsNullOrWhiteSpace(numero))
                            {
                                Console.Clear();
                                mensagemCadastro();
                                Console.WriteLine("O número da casa não pode estar vazio. Por favor, digite novamente.");
                                Thread.Sleep(1500);
                                Console.Clear();
                                mensagemCadastro();
                                Console.WriteLine($"Rua: {rua}");
                                continue;
                            }
                            else
                            {
                                break; // Sai do loop se o nome da rua não estiver vazio
                            }
                        }

                        while (true)
                        {
                            Console.Write("Digite o complemento: ");
                            complemento = Console.ReadLine();

                            if (string.IsNullOrWhiteSpace(complemento))
                            {
                                Console.Clear();
                                mensagemCadastro();
                                Console.WriteLine("O complemento não pode estar vazio. Por favor, digite novamente.");
                                Thread.Sleep(1500);
                                Console.Clear();
                                mensagemCadastro();
                                Console.WriteLine($"Rua: {rua}\nNúmero: {numero}");
                                continue;
                            }
                            else
                            {
                                break; // Sai do loop se o nome da rua não estiver vazio
                            }
                        }

                        while (true)
                        {
                            Console.Write("Digite o CEP: ");
                            cep = Console.ReadLine();

                            if (string.IsNullOrWhiteSpace(cep))
                            {
                                Console.Clear();
                                mensagemCadastro();
                                Console.WriteLine("O CEP não pode estar vazio. Por favor, digite novamente.");
                                Thread.Sleep(1500);
                                Console.Clear();
                                mensagemCadastro();
                                Console.WriteLine($"Rua: {rua}\nNúmero: {numero}\nComplemento:{complemento}");
                                continue;
                            }
                            else
                            {
                                break; // Sai do loop se o nome da rua não estiver vazio
                            }
                        }

                        Console.Clear();
                        mensagemCadastro();

                        while (true)
                        {
                            Console.Write("Digite a senha: ");
                            senhaUsuario = "";
                            mascaraSenha(ref senhaUsuario);

                            if (string.IsNullOrWhiteSpace(senhaUsuario))
                            {
                                Console.Clear();
                                mensagemCadastro();
                                Console.WriteLine("A senha não pode estar vazia. Por favor, digite novamente.");
                                Thread.Sleep(1500);
                                Console.Clear();
                                mensagemCadastro();
                                continue; // Retorna ao início do loop para solicitar a senha novamente
                            }

                            Console.Write("Confirme sua senha: ");
                            confirmarSenha = "";
                            mascaraSenha(ref confirmarSenha);

                            if (senhaUsuario == confirmarSenha)
                            {
                                // Se as senhas conferem, sai do loop
                                break;
                            }
                            else
                            {
                                Console.Clear();
                                mensagemCadastro();
                                Console.WriteLine("As senhas não conferem. Por favor, tente novamente.");
                                Thread.Sleep(1500);
                                Console.Clear();
                                mensagemCadastro();
                            }
                        }

                        // Se as senhas conferem e nenhum campo obrigatório está vazio, insere o novo usuário na tabela Usuario
                        string inserirUsuarioSql = "INSERT INTO Usuario (nome, email, senha) OUTPUT INSERTED.idUsuario VALUES (@nome, @Email, @senha)";
                        SqlCommand cmdInserirUsuario = new SqlCommand(inserirUsuarioSql, conexao);
                        cmdInserirUsuario.Parameters.AddWithValue("@nome", nomeCompleto);
                        cmdInserirUsuario.Parameters.AddWithValue("@Email", email);
                        cmdInserirUsuario.Parameters.AddWithValue("@senha", senhaUsuario);

                        int idUsuario = (int)cmdInserirUsuario.ExecuteScalar();

                        // Insere o endereço na tabela Endereco
                        string inserirEnderecoSql = "INSERT INTO Endereco (rua, numero, complemento, cep) OUTPUT INSERTED.idEndereco VALUES (@rua, @numero, @complemento, @cep)";
                        SqlCommand cmdInserirEndereco = new SqlCommand(inserirEnderecoSql, conexao);
                        cmdInserirEndereco.Parameters.AddWithValue("@rua", rua);
                        cmdInserirEndereco.Parameters.AddWithValue("@numero", numero);
                        cmdInserirEndereco.Parameters.AddWithValue("@complemento", complemento);
                        cmdInserirEndereco.Parameters.AddWithValue("@cep", cep);

                        int idEndereco = (int)cmdInserirEndereco.ExecuteScalar();

                        // Insere o novo cliente na tabela Cliente com a chave estrangeira idUsuario e idEndereco
                        string inserirClienteSql = "INSERT INTO Cliente (idUsuario, idEndereco, cpf, telefone) VALUES (@idUsuario, @idEndereco, @cpf, @telefone)";
                        SqlCommand cmdInserirCliente = new SqlCommand(inserirClienteSql, conexao);
                        cmdInserirCliente.Parameters.AddWithValue("@idUsuario", idUsuario);
                        cmdInserirCliente.Parameters.AddWithValue("@idEndereco", idEndereco);
                        cmdInserirCliente.Parameters.AddWithValue("@cpf", cpf);
                        cmdInserirCliente.Parameters.AddWithValue("@telefone", telefone);
                        cmdInserirCliente.ExecuteNonQuery();

                        Console.Clear();
                        mensagemCadastro();
                        Console.WriteLine("Cadastro realizado com sucesso");
                        Thread.Sleep(1500);
                        Console.Clear();
                        menuInicial();
                    }
                }

                void login()
                {
                    Console.Clear();
                    mensagemLogin();

                    Console.Write("Digite o seu e-mail: ");
                    string email = Console.ReadLine();
                    Console.Write("Digite a sua senha: ");

                    string senha = "";
                    mascaraSenha(ref senha);

                    using (SqlConnection conexao = new SqlConnection(conString))
                    {
                        conexao.Open();

                        // Verificar se o usuário é um cliente
                        string consultaCliente = "SELECT senha FROM Usuario WHERE email = @Email";
                        SqlCommand cmdCliente = new SqlCommand(consultaCliente, conexao);
                        cmdCliente.Parameters.AddWithValue("@Email", email);

                        SqlDataReader readerCliente = cmdCliente.ExecuteReader();

                        bool isCliente = readerCliente.HasRows;
                        readerCliente.Close();

                        if (isCliente)
                        {
                            // Verificar se o usuário também é um administrador
                            string consultaAdmin = "SELECT COUNT(*) FROM Administrador WHERE idUsuario = (SELECT idUsuario FROM Usuario WHERE email = @Email)";
                            SqlCommand cmdAdmin = new SqlCommand(consultaAdmin, conexao);
                            cmdAdmin.Parameters.AddWithValue("@Email", email);

                            int isAdmin = (int)cmdAdmin.ExecuteScalar();

                            Console.Clear();
                            mensagemLogin();

                            if (isAdmin > 0)
                            {
                                // Se o usuário for um administrador, redirecionar para o menu admin
                                Console.WriteLine("Logado com sucesso...");
                                emailLogado = email; // Atribuir o valor do emailLogado quando o usuário fizer login com sucesso
                                Thread.Sleep(1000);
                                Console.Clear();
                                ProgramaGeral programaGeral = new ProgramaGeral();
                                programaGeral.menuPrincipalAdmin(emailLogado); // Passa o emailLogado como argumento
                            }
                            else
                            {
                                // Se o usuário não for um administrador, redirecionar para o menu cliente
                                Console.WriteLine("Logado com sucesso...");
                                emailLogado = email; // Atribuir o valor do emailLogado quando o usuário fizer login com sucesso
                                Thread.Sleep(1000);
                                Console.Clear();
                                ProgramaGeral programaGeral = new ProgramaGeral();
                                programaGeral.menuPrincipalCliente(emailLogado); // Passa o emailLogado como argumento
                            }

                            return; // Sai do método após redirecionar para o menu correspondente
                        }
                        else
                        {
                            Console.Clear();
                            mensagemLogin();
                            Console.WriteLine("Usuário ou senha inválidos. Verifique o e-mail e a senha!");
                            Thread.Sleep(1500);
                            Console.Clear();
                            menuInicial();
                        }
                    }
                }


            }

            public string getEmailLogado()
            {
                return emailLogado;
            }
        }

        public class ProgramaGeral
        {
            public void comandoInexistente()
            {
                Console.Clear();
                menuPrincipalMensagem();
                Console.WriteLine("Desculpe, mas este comando/opção não existe");
                Thread.Sleep(1500);
                Console.Clear();
            }

            public void menuPrincipalMensagem()
            {
                Console.WriteLine("\t\t\t▄██▄▄██▄ ▄█████ ██████▄ ██   ██   ▄██████ ██     ▄█████▄ █████▄ ▄████▄ ██    \r\n" +
                                  "\t\t\t██ ██ ██ ██     ██   ██ ██   ██   ██      ██     ██   ██ ██  ██ ██  ██ ██    \r\n" +
                                  "\t\t\t██ ██ ██ █████  ██   ██ ██   ██   ██  ███ ██     ██   ██ █████  ██  ██ ██    \r\n" +
                                  "\t\t\t██ ██ ██ ██     ██   ██ ██   ██   ██   ██ ██     ██   ██ ██  ██ ██████ ██    \r\n" +
                                  "\t\t\t██ ██ ██ ▀█████ ██   ██ ▀█████▀   ▀█████▀ ██████ ▀█████▀ █████▀ ██  ██ █████ \r\n");
            }

            public void menuPrincipalCliente(string emailLogado)
            {
                TelaLogin tl = new TelaLogin();
                menuPrincipalMensagem();

                Console.WriteLine("1 - Visualizar produtos");
                Console.WriteLine("2 - Meu carrinho");
                Console.WriteLine("3 - Meu perfil");
                Console.WriteLine("4 - Meus pedidos");
                Console.WriteLine("0 - Sair");
                Console.Write("\nDigite a opção desejada: ");
                string op = Console.ReadLine();

                switch (op)
                {
                    case "1":
                        MostrarProdutos(emailLogado);
                        Console.ReadKey();
                        break;
                    case "2":
                        MeuCarrinho(emailLogado);
                        Console.ReadKey();
                        break;
                    case "3":
                        AlterarPerfil(emailLogado);
                        Console.ReadLine();
                        break;
                    case "4":
                        MeusPedidos(emailLogado);
                        Console.ReadKey();
                        break;
                    case "0":
                        Console.Clear();
                        tl.menuInicial(); // Chama a função menuInicial para retornar ao login
                        break;
                    default:
                        comandoInexistente();
                        menuPrincipalCliente(emailLogado);
                        break;
                }
            }

            void mensagemProdutos()
            {
                Console.WriteLine("\t\t\t█████▄ █████▄ ▄█████▄ ██████▄ ██   ██ ████████ ▄█████▄ ▄██████\r\n" +
                                  "\t\t\t██  ██ ██  ██ ██   ██ ██   ██ ██   ██    ██    ██   ██ ██     \r\n" +
                                  "\t\t\t█████▀ █████▀ ██   ██ ██   ██ ██   ██    ██    ██   ██ ▀█████▄\r\n" +
                                  "\t\t\t██     ██  ██ ██   ██ ██   ██ ██   ██    ██    ██   ██      ██\r\n" +
                                  "\t\t\t██     ██  ██ ▀█████▀ ██████▀ ▀█████▀    ██    ▀█████▀ ██████▀\r\n");
            }

            void mensagemCarrinho()
            {
                Console.Clear();
                Console.WriteLine("\t\t\t▄█████ ▄████▄ █████▄ █████▄ ██████ ██████▄ ██   ██ ▄█████▄\r\n" +
                                  "\t\t\t██     ██  ██ ██  ██ ██  ██   ██   ██   ██ ██   ██ ██   ██\r\n" +
                                  "\t\t\t██     ██  ██ █████▀ █████▀   ██   ██   ██ ███████ ██   ██\r\n" +
                                  "\t\t\t██     ██████ ██  ██ ██  ██   ██   ██   ██ ██   ██ ██   ██\r\n" +
                                  "\t\t\t▀█████ ██  ██ ██  ██ ██  ██ ██████ ██   ██ ██   ██ ▀█████▀\r\n");
            }

            void mensagemPedidos()
            {
                Console.Clear();
                Console.WriteLine("\t\t\t█████▄ ▄█████ ██████▄ ██████ ██████▄ ▄█████▄ ▄██████\r\n" +
                                  "\t\t\t██  ██ ██     ██   ██   ██   ██   ██ ██   ██ ██     \r\n" +
                                  "\t\t\t█████▀ █████  ██   ██   ██   ██   ██ ██   ██ ▀█████▄\r\n" +
                                  "\t\t\t██     ██     ██   ██   ██   ██   ██ ██   ██      ██\r\n" +
                                  "\t\t\t██     ▀█████ ██████▀ ██████ ██████▀ ▀█████▀ ██████▀");
            }

            void mensagemPerfil()
            {
                Console.Clear();
                Console.WriteLine("\t\t\t▄██▄▄██▄ ▄█████ ██   ██   █████▄ ▄█████ █████▄ ██████ ██████ ██    \r\n" +
                                  "\t\t\t██ ██ ██ ██     ██   ██   ██  ██ ██     ██  ██ ██       ██   ██    \r\n" +
                                  "\t\t\t██ ██ ██ █████  ██   ██   █████▀ █████  █████▀ █████    ██   ██    \r\n" +
                                  "\t\t\t██ ██ ██ ██     ██   ██   ██     ██     ██  ██ ██       ██   ██    \r\n" +
                                  "\t\t\t██ ██ ██ ▀█████ ▀█████▀   ██     ▀█████ ██  ██ ██     ██████ █████\n");
            }

            void mensagemEstoque()
            {
                Console.Clear();
                Console.WriteLine("\t\t\t▄█████ ▄██████ ████████ ▄█████▄ ▄█████▄ ██   ██ ▄█████\r\n" +
                                  "\t\t\t██     ██         ██    ██   ██ ██   ██ ██   ██ ██    \r\n" +
                                  "\t\t\t█████  ▀█████▄    ██    ██   ██ ██   ██ ██   ██ █████ \r\n" +
                                  "\t\t\t██          ██    ██    ██   ██ ██   ██ ██   ██ ██    \r\n" +
                                  "\t\t\t▀█████ ██████▀    ██    ▀█████▀ ▀█████▀ ▀█████▀ ▀█████\n");
            }

            void mensagemFornecedor()
            {
                Console.WriteLine("\t\t\t██████ ▄█████▄ █████▄ ██████▄ ▄█████ ▄█████ ▄█████ ██████▄ ▄█████▄ █████▄\r\n" +
                                  "\t\t\t██     ██   ██ ██  ██ ██   ██ ██     ██     ██     ██   ██ ██   ██ ██  ██\r\n" +
                                  "\t\t\t█████  ██   ██ █████▀ ██   ██ █████  ██     █████  ██   ██ ██   ██ █████▀\r\n" +
                                  "\t\t\t██     ██   ██ ██  ██ ██   ██ ██     ██     ██     ██   ██ ██   ██ ██  ██\r\n" +
                                  "\t\t\t██     ▀█████▀ ██  ██ ██   ██ ▀█████ ▀█████ ▀█████ ██████▀ ▀█████▀ ██  ██\n");
            }

            void mensagemVendas()
            {
                Console.WriteLine("\t\t\t██████ ▄█████▄ █████▄ ██████▄ ▄█████ ▄█████ ▄█████ ██████▄ ▄█████▄ █████▄\r\n" +
                                  "\t\t\t██     ██   ██ ██  ██ ██   ██ ██     ██     ██     ██   ██ ██   ██ ██  ██\r\n" +
                                  "\t\t\t█████  ██   ██ █████▀ ██   ██ █████  ██     █████  ██   ██ ██   ██ █████▀\r\n" +
                                  "\t\t\t██     ██   ██ ██  ██ ██   ██ ██     ██     ██     ██   ██ ██   ██ ██  ██\r\n" +
                                  "\t\t\t██     ▀█████▀ ██  ██ ██   ██ ▀█████ ▀█████ ▀█████ ██████▀ ▀█████▀ ██  ██\n");
            }

            // opção 1 do switch
            private void MostrarProdutos(string emailLogado)
            {
                string conString = @"Data Source=DESKTOP-UUDL5AC\SQLEXPRESS;Initial Catalog=SafeEats;Integrated Security=True;";
                using (SqlConnection conexao = new SqlConnection(conString))
                {
                    conexao.Open();
                    string consulta = "SELECT idProduto, nome, descricao, preco FROM Produto WHERE quantidade > 0";
                    SqlCommand cmd = new SqlCommand(consulta, conexao);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Console.Clear();
                    mensagemProdutos();

                    while (reader.Read())
                    {
                        int idProduto = reader.GetInt32(0);
                        string nome = reader.GetString(1);
                        string descricao = reader.GetString(2);
                        decimal preco = reader.GetDecimal(3);

                        Console.WriteLine($"ID: {idProduto} | Nome: {nome}");
                    }

                    reader.Close();
                }

                string produtoEscolhido;
                Console.Write("\nEscolha um produto pelo ID: ");
                produtoEscolhido = Console.ReadLine();

                // Lógica para exibir os detalhes do produto escolhido
                using (SqlConnection conexao = new SqlConnection(conString))
                {
                    conexao.Open();
                    string consulta = "SELECT nome, descricao, preco FROM Produto WHERE idProduto = @id AND quantidade > 0";
                    SqlCommand cmd = new SqlCommand(consulta, conexao);
                    cmd.Parameters.AddWithValue("@id", produtoEscolhido);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.Clear();
                            mensagemProdutos();

                            string nome = reader.GetString(0);
                            string descricao = reader.GetString(1);
                            decimal preco = reader.GetDecimal(2);

                            Console.WriteLine($"\n[PRODUTO - {nome}] Detalhes\n");
                            Console.WriteLine($"Nome: {nome}");
                            Console.WriteLine($"Descrição: {descricao}");
                            Console.WriteLine($"Preço: {preco:C2}");
                        }
                    }
                    else
                    {
                        Console.Clear();
                        mensagemProdutos();
                        Console.WriteLine("Produto não encontrado.");
                        Thread.Sleep(1500);
                        Console.Clear();
                        menuPrincipalCliente(emailLogado);
                        return;
                    }

                    reader.Close();
                }

                // Pergunta se deseja adicionar ao carrinho
                Console.Write("\nDeseja adicionar este produto ao carrinho? (S/N): ");
                string resposta = Console.ReadLine().ToUpper();

                if (resposta.ToUpper() == "S")
                {
                    Console.Write("Quantidade desejada: ");
                    if (int.TryParse(Console.ReadLine(), out int quantidade) && quantidade > 0)
                    {
                        AdicionarAoCarrinho(produtoEscolhido, emailLogado, quantidade);
                    }
                    else
                    {
                        Console.WriteLine("Quantidade inválida. Produto não adicionado ao carrinho.");
                    }
                }
                else
                {
                    Console.Clear();
                    mensagemProdutos();
                    Console.WriteLine("Produto NÃO adicionado ao carrinho!");
                    Thread.Sleep(1000);
                    Console.Clear();
                    MostrarProdutos(emailLogado);
                    return;
                }

                Console.ReadLine(); // Aguarda o usuário antes de retornar ao menu
            }

            private void AdicionarAoCarrinho(string idProduto, string emailUsuario, int quantidade)
            {
                string conString = @"Data Source=DESKTOP-UUDL5AC\SQLEXPRESS;Initial Catalog=SafeEats;Integrated Security=True;";

                using (SqlConnection conexao = new SqlConnection(conString))
                {
                    conexao.Open();

                    // Busca o idUsuario associado ao email fornecido
                    string buscarIdUsuarioSql = "SELECT idUsuario FROM Usuario WHERE email = @Email";
                    SqlCommand cmdBuscarIdUsuario = new SqlCommand(buscarIdUsuarioSql, conexao);
                    cmdBuscarIdUsuario.Parameters.AddWithValue("@Email", emailUsuario);
                    int idUsuario = (int)cmdBuscarIdUsuario.ExecuteScalar();

                    // Busca o idCliente associado ao email fornecido
                    string buscarIdClienteSql = "SELECT idCliente FROM Cliente WHERE idUsuario = @idUsuario"; 
                    SqlCommand cmdBuscarIdCliente = new SqlCommand(buscarIdClienteSql, conexao);
                    cmdBuscarIdCliente.Parameters.AddWithValue("@idUsuario", idUsuario);
                    int IdCliente = (int)cmdBuscarIdCliente.ExecuteScalar();


                    // Busca a quantidade em estoque do produto
                    string buscarQuantidadeSql = "SELECT quantidade FROM Produto WHERE idProduto = @idProduto";
                    SqlCommand cmdBuscarQuantidade = new SqlCommand(buscarQuantidadeSql, conexao);
                    cmdBuscarQuantidade.Parameters.AddWithValue("@idProduto", idProduto);
                    int quantidadeEmEstoque = (int)cmdBuscarQuantidade.ExecuteScalar();

                    while (quantidade > quantidadeEmEstoque)
                    {
                        Console.Clear();
                        mensagemProdutos();
                        Console.WriteLine($"Quantidade desejada maior que a quantidade em estoque. Quantidade disponível: {quantidadeEmEstoque}");
                        Console.Write("Digite a quantidade desejada: ");
                        quantidade = int.Parse(Console.ReadLine());
                    }

                    // Verifica se o produto já está no carrinho do usuário
                    string verificarProdutoSql = "SELECT COUNT(*) FROM ItemCarrinho WHERE idProduto = @idProduto AND idUsuario = @idUsuario";
                    SqlCommand cmdVerificarProduto = new SqlCommand(verificarProdutoSql, conexao);
                    cmdVerificarProduto.Parameters.AddWithValue("@idProduto", idProduto);
                    cmdVerificarProduto.Parameters.AddWithValue("@idUsuario", idUsuario);
                    int count = (int)cmdVerificarProduto.ExecuteScalar();

                    if (count > 0)
                    {
                        // Se o produto já estiver no carrinho, atualiza a quantidade
                        string atualizarQuantidadeSql = "UPDATE ItemCarrinho SET quantidade = quantidade + @quantidade, subtotal = subtotal + (@quantidade * precoUnitario) WHERE idProduto = @idProduto AND idUsuario = @idUsuario";
                        SqlCommand cmdAtualizarQuantidade = new SqlCommand(atualizarQuantidadeSql, conexao);
                        cmdAtualizarQuantidade.Parameters.AddWithValue("@idProduto", idProduto);
                        cmdAtualizarQuantidade.Parameters.AddWithValue("@idUsuario", idUsuario);
                        cmdAtualizarQuantidade.Parameters.AddWithValue("@quantidade", quantidade);
                        int rowsAffected = cmdAtualizarQuantidade.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.Clear();
                            mensagemProdutos();
                            Console.WriteLine($"Quantidade do produto atualizada no carrinho: + {quantidade}");
                            Thread.Sleep(1500);
                            Console.Clear();
                            MostrarProdutos(emailUsuario);
                        }
                        else
                        {
                            Console.WriteLine("Falha ao atualizar a quantidade no carrinho.");
                        }
                    }
                    else
                    {
                        // Insere o produto no carrinho do usuário
                        string inserirItemSql = "INSERT INTO ItemCarrinho (idProduto, idUsuario, quantidade, precoUnitario, subtotal) VALUES (@idProduto, @idUsuario, @quantidade, @precoUnitario, @subtotal); SELECT SCOPE_IDENTITY()";
                        SqlCommand cmdInserirItem = new SqlCommand(inserirItemSql, conexao);
                        cmdInserirItem.Parameters.AddWithValue("@idProduto", idProduto);
                        cmdInserirItem.Parameters.AddWithValue("@idUsuario", idUsuario);
                        cmdInserirItem.Parameters.AddWithValue("@quantidade", quantidade);

                        // Busca o preço unitário do produto
                        string buscarPrecoSql = "SELECT preco FROM Produto WHERE idProduto = @idProduto";
                        SqlCommand cmdBuscarPreco = new SqlCommand(buscarPrecoSql, conexao);
                        cmdBuscarPreco.Parameters.AddWithValue("@idProduto", idProduto);
                        decimal precoUnitario = (decimal)cmdBuscarPreco.ExecuteScalar();
                        cmdInserirItem.Parameters.AddWithValue("@precoUnitario", precoUnitario);

                        // Calcula o subtotal
                        decimal subtotal = quantidade * precoUnitario;
                        cmdInserirItem.Parameters.AddWithValue("@subtotal", subtotal);

                        int idItemCarrinho = Convert.ToInt32(cmdInserirItem.ExecuteScalar());

                        // Insere o idItemCarrinho e idUsuario na tabela CarrinhoCompra
                        string inserirCarrinhoSql = "INSERT INTO CarrinhoCompra (idCliente, idItemCarrinho, statusCarrinho) VALUES (@idCliente, @idItemCarrinho, 'Ativo')";
                        SqlCommand cmdInserirCarrinho = new SqlCommand(inserirCarrinhoSql, conexao);
                        cmdInserirCarrinho.Parameters.AddWithValue("@idCliente", IdCliente);
                        cmdInserirCarrinho.Parameters.AddWithValue("@idItemCarrinho", idItemCarrinho);
                        int rowsAffected = cmdInserirCarrinho.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.Clear();
                            mensagemProdutos();
                            Console.WriteLine("Produto adicionado ao carrinho com sucesso!");
                            Thread.Sleep(1000);
                            Console.Clear();
                            menuPrincipalCliente(emailUsuario);
                        }
                        else
                        {
                            Console.WriteLine("Falha ao adicionar o ID do item do carrinho ao carrinho.");
                        }
                    }
                }
            }

            //opção 2 do switch
            private void MeuCarrinho(string email)
            {
                mensagemCarrinho();

                string conString = @"Data Source=DESKTOP-UUDL5AC\SQLEXPRESS;Initial Catalog=SafeEats;Integrated Security=True;";
                decimal totalCarrinho = 0;

                using (SqlConnection conexao = new SqlConnection(conString))
                {
                    conexao.Open();

                    // Buscar o idUsuario associado ao email fornecido
                    string buscarIdUsuarioSql = "SELECT idUsuario FROM Usuario WHERE email = @Email";
                    SqlCommand cmdBuscarIdUsuario = new SqlCommand(buscarIdUsuarioSql, conexao);
                    cmdBuscarIdUsuario.Parameters.AddWithValue("@Email", email);
                    int idUsuario = (int)cmdBuscarIdUsuario.ExecuteScalar();

                    // Consulta para obter os detalhes dos produtos no carrinho com status ativo
                    string consulta = @"SELECT ic.idItemCarrinho, p.nome, p.descricao, ic.quantidade, ic.precoUnitario, ic.subtotal
                    FROM CarrinhoCompra cc
                    INNER JOIN ItemCarrinho ic ON cc.idItemCarrinho = ic.idItemCarrinho
                    INNER JOIN Produto p ON ic.idProduto = p.idProduto
                    WHERE ic.idUsuario = @IdCliente AND cc.statusCarrinho = 'Ativo'";

                    SqlCommand cmd = new SqlCommand(consulta, conexao);
                    cmd.Parameters.AddWithValue("@IdCliente", idUsuario);

                    // Busca o idCliente associado ao email fornecido
                    string buscarIdClienteSql = "SELECT idCliente FROM Cliente WHERE idUsuario = @idUsuario";
                    SqlCommand cmdBuscarIdCliente = new SqlCommand(buscarIdClienteSql, conexao);
                    cmdBuscarIdCliente.Parameters.AddWithValue("@idUsuario", idUsuario);
                    int IdCliente = (int)cmdBuscarIdCliente.ExecuteScalar();

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int idItemCarrinho = reader.GetInt32(0);
                            string nomeProduto = reader.GetString(1);
                            string descricaoProduto = reader.GetString(2);
                            int quantidade = reader.GetInt32(3);
                            decimal precoUnitario = reader.GetDecimal(4);
                            decimal subtotal = reader.GetDecimal(5);

                            Console.WriteLine($"[CARRINHO - {nomeProduto}] Detalhes");
                            Console.WriteLine($"ID: {idItemCarrinho}");
                            Console.WriteLine($"Nome: {nomeProduto}");
                            Console.WriteLine($"Descrição: {descricaoProduto}");
                            Console.WriteLine($"Quantidade: {quantidade}");
                            Console.WriteLine($"Preço Unitário: {precoUnitario:C2}");
                            Console.WriteLine($"Subtotal: {subtotal:C2}\n");

                            totalCarrinho += subtotal;
                        }

                        Console.WriteLine($"Total do Carrinho: {totalCarrinho:C2}");

                        Console.WriteLine("\n1 - Criar pedido\n2 - Alterar produto\n3 - Remover produto");
                        Console.Write("\nDigite a opção: ");
                        string resposta = Console.ReadLine();

                        if (resposta == "1")
                        {
                            // Criar um pedido com base nos itens do carrinho
                            Console.Clear();
                            mensagemProdutos();
                            Console.WriteLine("Criando pedido...");
                            Thread.Sleep(2000);
                            Console.Clear();
                            CriarPedido(IdCliente, email);
                        }
                        else if (resposta == "2")
                        {
                            // Alterando produto com base no idItemCarrinho
                            Console.Write("Qual o ID do Item?: ");
                            string idItemCarrinho = Console.ReadLine();

                            // Alterando produto com base no idItemCarrinho
                            Console.Write("Nova quantidade: ");
                            int novaQuantidade = Convert.ToInt32(Console.ReadLine());

                            AlterarQuantidade(idItemCarrinho, novaQuantidade, email);
                        }
                        else if (resposta == "3")
                        {
                            // Alterando produto com base no idItemCarrinho
                            Console.Write("Qual o ID do Item?: ");
                            string idItemCarrinho = Console.ReadLine();

                            RemoverItemCarrinho(idItemCarrinho, email);
                        }
                        else
                        {
                            Console.WriteLine("Continuando sem criar um pedido.");
                        }
                    }
                    else
                    {
                        Console.Clear();
                        mensagemProdutos();
                        Console.WriteLine("Não há produtos no carrinho.");
                        Thread.Sleep(1500);
                        Console.Clear();
                        menuPrincipalCliente(email);
                        return;
                    }
                }
            }

            private void AlterarQuantidade(string idItemCarrinho, int novaQuantidade, string emailLogado)
            {
                string conString = @"Data Source=DESKTOP-UUDL5AC\SQLEXPRESS;Initial Catalog=SafeEats;Integrated Security=True;";

                using (SqlConnection conexao = new SqlConnection(conString))
                {
                    conexao.Open();

                    if (novaQuantidade == 0)
                    {
                        // Se a nova quantidade for zero, remove o item do carrinho
                        RemoverItemCarrinho(idItemCarrinho, emailLogado);
                        return;
                    }

                    string atualizarQuantidadeSql = "UPDATE ItemCarrinho SET quantidade = @NovaQuantidade WHERE idItemCarrinho = @IdItemCarrinho";
                    SqlCommand cmdAtualizarQuantidade = new SqlCommand(atualizarQuantidadeSql, conexao);
                    cmdAtualizarQuantidade.Parameters.AddWithValue("@NovaQuantidade", novaQuantidade);
                    cmdAtualizarQuantidade.Parameters.AddWithValue("@IdItemCarrinho", idItemCarrinho);

                    int rowsAffected = cmdAtualizarQuantidade.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        mensagemCarrinho();
                        Console.WriteLine("Quantidade atualizada com sucesso!");
                        Thread.Sleep(1000);
                        Console.Clear();
                        menuPrincipalCliente(emailLogado);
                    }
                    else
                    {
                        Console.WriteLine("Falha ao atualizar a quantidade.");
                    }
                }
            }

            private void RemoverItemCarrinho(string idItemCarrinho, string emailLogado)
            {
                string conString = @"Data Source=DESKTOP-UUDL5AC\SQLEXPRESS;Initial Catalog=SafeEats;Integrated Security=True;";

                using (SqlConnection conexao = new SqlConnection(conString))
                {
                    conexao.Open();

                    // Remover o item do carrinho geral
                    string removerCarrinhoSql = "DELETE FROM CarrinhoCompra WHERE idItemCarrinho = @IdItemCarrinho";
                    SqlCommand cmdRemoverCarrinho = new SqlCommand(removerCarrinhoSql, conexao);
                    cmdRemoverCarrinho.Parameters.AddWithValue("@IdItemCarrinho", idItemCarrinho);

                    int rowsAffectedG = cmdRemoverCarrinho.ExecuteNonQuery();

                    // Remover o item do carrinho
                    string removerItemSql = "DELETE FROM ItemCarrinho WHERE idItemCarrinho = @IdItemCarrinho";
                    SqlCommand cmdRemoverItem = new SqlCommand(removerItemSql, conexao);
                    cmdRemoverItem.Parameters.AddWithValue("@IdItemCarrinho", idItemCarrinho);

                    int rowsAffectedC = cmdRemoverItem.ExecuteNonQuery();

                    if (rowsAffectedG > 0 && rowsAffectedC > 0)
                    {
                        mensagemCarrinho();
                        Console.WriteLine("Item removido do carrinho com sucesso!");

                        Thread.Sleep(1000);
                        Console.Clear();
                        menuPrincipalCliente(emailLogado);
                    }
                    else
                    {
                        Console.WriteLine("Falha ao remover o item do carrinho.");
                    }
                }
            }

            private void ListarFormasPagamento(SqlConnection conexao)
            {
                string query = "SELECT idFormaPagamento, nomeFormaPagamento FROM FormaPagamento";
                SqlCommand cmd = new SqlCommand(query, conexao);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int idFormaPagamento = reader.GetInt32(0);
                    string nomeFormaPagamento = reader.GetString(1);
                    Console.WriteLine($"{idFormaPagamento}. {nomeFormaPagamento}");
                }
                reader.Close();
            }

            private void ListarFormasEnvio(SqlConnection conexao)
            {
                string query = "SELECT idFormaEnvio, nomeFormaEnvio FROM FormaEnvio";
                SqlCommand cmd = new SqlCommand(query, conexao);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int idFormaEnvio = reader.GetInt32(0);
                    string nomeFormaEnvio = reader.GetString(1);
                    Console.WriteLine($"{idFormaEnvio}. {nomeFormaEnvio}");
                }
                reader.Close();
            }

            private void CriarPedido(int idCliente, string emailLogado)
            {
                string conString = @"Data Source=DESKTOP-UUDL5AC\SQLEXPRESS;Initial Catalog=SafeEats;Integrated Security=True;";

                using (SqlConnection conexao = new SqlConnection(conString))
                {
                    conexao.Open();
                    Console.Clear();
                    mensagemProdutos();

                    // Exibir as formas de pagamento disponíveis e solicitar ao usuário que escolha uma
                    Console.WriteLine("[PAGAMENTO] Formas de pagamento disponíveis\n");
                    ListarFormasPagamento(conexao);
                    Console.Write("\nEscolha a forma de pagamento pelo ID: ");
                    int idFormaPagamento = int.Parse(Console.ReadLine());

                    Console.Clear();
                    mensagemProdutos();

                    // Exibir as formas de envio disponíveis e solicitar ao usuário que escolha uma
                    Console.WriteLine("[ENVIOS] Formas de envio disponíveis\n");
                    ListarFormasEnvio(conexao);
                    Console.Write("\nEscolha a forma de envio pelo ID: ");
                    int idFormaEnvio = int.Parse(Console.ReadLine());

                    Console.Clear();
                    mensagemProdutos();

                    // Inserir pedido na tabela Pedido
                    string inserirPedidoSql = @"
                    INSERT INTO Pedido (idCliente, idFormaPagamento, idFormaEnvio, dtPedido, dtCriacao, statusPedido)
                    VALUES (@IdCliente, @IdFormaPagamento, @IdFormaEnvio, GETDATE(), GETDATE(), 'Em análise');
                    SELECT SCOPE_IDENTITY();"; // Retorna o ID do pedido inserido
                    SqlCommand cmdInserirPedido = new SqlCommand(inserirPedidoSql, conexao);
                    cmdInserirPedido.Parameters.AddWithValue("@IdCliente", idCliente);
                    cmdInserirPedido.Parameters.AddWithValue("@IdFormaPagamento", idFormaPagamento);
                    cmdInserirPedido.Parameters.AddWithValue("@IdFormaEnvio", idFormaEnvio);

                    int idPedido = Convert.ToInt32(cmdInserirPedido.ExecuteScalar());

                    if (idPedido > 0)
                    {
                        // Verifica se há produtos com quantidade suficiente no carrinho para criar o pedido
                        string verificarQuantidadeSql = @"
                        SELECT COUNT(*) 
                        FROM CarrinhoCompra cc
                        INNER JOIN ItemCarrinho ic ON cc.idItemCarrinho = ic.idItemCarrinho
                        INNER JOIN Produto p ON ic.idProduto = p.idProduto
                        WHERE cc.idCliente = @IdCliente AND cc.statusCarrinho = 'Ativo'
                        GROUP BY ic.idProduto
                        HAVING SUM(ic.quantidade) <= MAX(p.quantidade)";

                        SqlCommand cmdVerificarQuantidade = new SqlCommand(verificarQuantidadeSql, conexao);
                        cmdVerificarQuantidade.Parameters.AddWithValue("@IdCliente", idCliente);

                        object result = cmdVerificarQuantidade.ExecuteScalar();

                        if (result != null && (int)result == 1)
                        {
                            // Mover itens do carrinho para o pedido
                            string moverItensSql = @"
                            INSERT INTO PedidoItem (codPedido, idProduto, quantidade, precoUnitario)
                            SELECT @CodPedido, ic.idProduto, ic.quantidade, ic.precoUnitario
                            FROM CarrinhoCompra cc
                            INNER JOIN ItemCarrinho ic ON cc.idItemCarrinho = ic.idItemCarrinho
                            WHERE cc.idCliente = @IdCliente AND cc.statusCarrinho = 'Ativo';
                            UPDATE CarrinhoCompra SET statusCarrinho = 'Processado' WHERE idCliente = @IdCliente;";
                            SqlCommand cmdMoverItens = new SqlCommand(moverItensSql, conexao);
                            cmdMoverItens.Parameters.AddWithValue("@CodPedido", idPedido);
                            cmdMoverItens.Parameters.AddWithValue("@IdCliente", idCliente);

                            cmdMoverItens.ExecuteNonQuery();

                            // Atualizar a quantidade do produto e verificar se a quantidade chegou a zero
                            string atualizarQuantidadeSql = @"
                            UPDATE p
                            SET p.quantidade = p.quantidade - ic.quantidade
                            OUTPUT inserted.nome, inserted.quantidade
                            FROM Produto p
                            INNER JOIN ItemCarrinho ic ON p.idProduto = ic.idProduto
                            INNER JOIN CarrinhoCompra cc ON ic.idItemCarrinho = cc.idItemCarrinho
                            WHERE cc.idCliente = @IdCliente AND cc.statusCarrinho = 'Processado';";

                            SqlCommand cmdAtualizarQuantidade = new SqlCommand(atualizarQuantidadeSql, conexao);
                            cmdAtualizarQuantidade.Parameters.AddWithValue("@IdCliente", idCliente);

                            SqlDataReader reader = cmdAtualizarQuantidade.ExecuteReader();

                            while (reader.Read())
                            {
                                string nomeProduto = reader["nome"].ToString();
                                int quantidade = (int)reader["quantidade"];
                                int validarPedido = 1;

                                if (validarPedido != 1)
                                {
                                    Console.WriteLine($"Produto {nomeProduto} sem quantidade disponível.");
                                }
                            }

                            reader.Close();

                            Console.Clear();
                            mensagemProdutos();
                            Console.WriteLine("Pedido criado com sucesso!");
                            Thread.Sleep(1500);
                            Console.Clear();
                            menuPrincipalCliente(emailLogado);
                        }
                        else
                        {
                            Console.WriteLine("Não há quantidade suficiente de algum dos produtos no estoque para criar o pedido.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Erro ao criar o pedido.");
                    }
                }
            }

            // opção 3 do switch
            private void AlterarPerfil(string email)
            {
                Console.Clear();
                mensagemPerfil();

                string conString = @"Data Source=DESKTOP-UUDL5AC\SQLEXPRESS;Initial Catalog=SafeEats;Integrated Security=True;";
                using (SqlConnection conexao = new SqlConnection(conString))
                {
                    conexao.Open();
                    string consulta = @"SELECT nome, email, cpf, telefone, rua, numero, complemento, cep FROM Usuario u INNER JOIN Cliente c ON u.idUsuario = c.idUsuario INNER JOIN Endereco e ON c.idEndereco = e.idEndereco WHERE email = @Email";
                    SqlCommand cmd = new SqlCommand(consulta, conexao);
                    cmd.Parameters.AddWithValue("@Email", email);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"Nome completo: {reader["nome"]}");
                            Console.WriteLine($"CPF: {reader["cpf"]}");
                            Console.WriteLine($"E-mail: {reader["email"]}");
                            Console.WriteLine($"Telefone: {reader["telefone"]}");
                            Console.WriteLine($"Rua: {reader["rua"]}");
                            Console.WriteLine($"Número: {reader["numero"]}");
                            Console.WriteLine($"Complemento: {reader["complemento"]}");
                            Console.WriteLine($"CEP: {reader["cep"]}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Usuário não encontrado.");
                    }

                    Console.Write("\nDeseja alterar o perfil? (s/n): ");
                    string resposta = Console.ReadLine();

                    if (resposta.ToLower() == "n")
                    {
                        Console.Clear();
                        menuPrincipalCliente(email);
                        return;
                    }
                    else if (resposta.ToLower() == "s")
                    {
                        Console.Clear();
                        mensagemPerfil();
                        Console.Write("Qual dado você deseja alterar?\n1 - Dados pessoais\n2 - Dados de Endereço\n3 - Dados de Senha\n\nEscolha um número: ");
                        string opcao = Console.ReadLine().ToLower();

                        switch (opcao)
                        {
                            case "1":
                                AlterarDadosPessoais(email);
                                break;
                            case "2":
                                AlterarEndereco(email);
                                break;
                            case "3":
                                AlterarSenha(email);
                                break;
                            default:
                                Console.WriteLine("Opção inválida.");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Opção inválida.");
                    }
                }
            }

            // Função para mascarar senha
            void mascaraSenha(ref string senha)
            {
                ConsoleKeyInfo key;
                do
                {
                    key = Console.ReadKey(true);
                    if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                    {
                        senha += key.KeyChar;
                        Console.Write("*");
                    }
                    else if (key.Key == ConsoleKey.Backspace && senha.Length > 0)
                    {
                        senha = senha.Substring(0, senha.Length - 1);
                        Console.Write("\b \b");
                    }
                } while (key.Key != ConsoleKey.Enter);

                Console.WriteLine(); // Nova linha após a senha
            }

            // Função para alterar dados pessoais
            private void AlterarDadosPessoais(string email)
            {
                Console.Clear();
                mensagemPerfil();

                Console.Write("Digite o novo nome:");
                string novoNome = Console.ReadLine();

                Console.Write("Digite o novo CPF:");
                string novoCPF = Console.ReadLine();

                Console.Write("Digite o novo telefone:");
                string novoTelefone = Console.ReadLine();

                // Conectar ao banco de dados e executar a atualização
                string conString = @"Data Source=DESKTOP-UUDL5AC\SQLEXPRESS;Initial Catalog=SafeEats;Integrated Security=True;";
                using (SqlConnection conexao = new SqlConnection(conString))
                {
                    conexao.Open();

                    string atualizarDadosSql = @"UPDATE Usuario 
                                     SET nome = @NovoNome, 
                                         cpf = @NovoCPF, 
                                         telefone = @NovoTelefone 
                                     WHERE email = @Email";

                    SqlCommand cmdAtualizarDados = new SqlCommand(atualizarDadosSql, conexao);
                    cmdAtualizarDados.Parameters.AddWithValue("@NovoNome", novoNome);
                    cmdAtualizarDados.Parameters.AddWithValue("@NovoCPF", novoCPF);
                    cmdAtualizarDados.Parameters.AddWithValue("@NovoTelefone", novoTelefone);
                    cmdAtualizarDados.Parameters.AddWithValue("@Email", email);

                    int rowsAffected = cmdAtualizarDados.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Dados pessoais atualizados com sucesso!");
                    }
                    else
                    {
                        Console.WriteLine("Falha ao atualizar os dados pessoais.");
                    }
                }
            }

            // Função para alterar o endereço
            private void AlterarEndereco(string email)
            {
                Console.Clear();
                mensagemPerfil();

                Console.Write("Digite a nova rua:");
                string novaRua = Console.ReadLine();

                Console.Write("Digite o novo número:");
                string novoNumero = Console.ReadLine();

                Console.Write("Digite o novo complemento:");
                string novoComplemento = Console.ReadLine();

                Console.WriteLine("Digite o novo CEP:");
                string novoCEP = Console.ReadLine();

                // Conectar ao banco de dados e executar a atualização
                string conString = @"Data Source=DESKTOP-UUDL5AC\SQLEXPRESS;Initial Catalog=SafeEats;Integrated Security=True;";
                using (SqlConnection conexao = new SqlConnection(conString))
                {
                    conexao.Open();

                    string atualizarEnderecoSql = @"UPDATE Endereco 
                                        SET rua = @NovaRua, 
                                            numero = @NovoNumero, 
                                            complemento = @NovoComplemento, 
                                            cep = @NovoCEP 
                                        WHERE idEndereco = (SELECT idEndereco FROM Cliente WHERE idUsuario = (SELECT idUsuario FROM Usuario WHERE email = @Email))";

                    SqlCommand cmdAtualizarEndereco = new SqlCommand(atualizarEnderecoSql, conexao);
                    cmdAtualizarEndereco.Parameters.AddWithValue("@NovaRua", novaRua);
                    cmdAtualizarEndereco.Parameters.AddWithValue("@NovoNumero", novoNumero);
                    cmdAtualizarEndereco.Parameters.AddWithValue("@NovoComplemento", novoComplemento);
                    cmdAtualizarEndereco.Parameters.AddWithValue("@NovoCEP", novoCEP);
                    cmdAtualizarEndereco.Parameters.AddWithValue("@Email", email);

                    int rowsAffected = cmdAtualizarEndereco.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Endereço atualizado com sucesso!");
                    }
                    else
                    {
                        Console.WriteLine("Falha ao atualizar o endereço.");
                    }
                }
            }

            // Função para alterar a senha
            private void AlterarSenha(string email)
            {
                Console.Clear();
                mensagemPerfil();


                Console.Write("Digite a nova senha: ");
                string novaSenha = "";
                mascaraSenha(ref novaSenha);

                Console.Write("Confirme a nova senha: ");
                string confirmarNovaSenha = "";
                mascaraSenha(ref confirmarNovaSenha);

                if (novaSenha == confirmarNovaSenha)
                {
                    // Conectar ao banco de dados e executar a atualização
                    string conString = @"Data Source=DESKTOP-UUDL5AC\SQLEXPRESS;Initial Catalog=SafeEats;Integrated Security=True;";
                    using (SqlConnection conexao = new SqlConnection(conString))
                    {
                        conexao.Open();

                        string atualizarSenhaSql = @"UPDATE Usuario 
                                         SET senha = @NovaSenha
                                         WHERE email = @Email";

                        SqlCommand cmdAtualizarSenha = new SqlCommand(atualizarSenhaSql, conexao);
                        cmdAtualizarSenha.Parameters.AddWithValue("@NovaSenha", novaSenha);
                        cmdAtualizarSenha.Parameters.AddWithValue("@Email", email);

                        int rowsAffected = cmdAtualizarSenha.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.Clear();
                            mensagemPerfil();
                            Console.WriteLine("Senha atualizada com sucesso!");
                            Thread.Sleep(1500);
                            Console.Clear();
                            menuPrincipalCliente(email);
                        }
                        else
                        {
                            Console.Clear();
                            mensagemPerfil();
                            Console.WriteLine("Falha ao atualizar a senha.");
                            Thread.Sleep(1500);
                            Console.Clear();
                            menuPrincipalCliente(email);
                        }
                    }
                }
                else
                {
                    Console.Clear();
                    mensagemPerfil();
                    Console.WriteLine("As senhas não conferem. Tente novamente!");
                    Thread.Sleep(1500);
                    Console.Clear();
                    AlterarSenha(email);
                }
            }

            //opção 4 do switch
            private void MeusPedidos(string emailLogado)
            {
                mensagemPedidos();
                string conString = @"Data Source=DESKTOP-UUDL5AC\SQLEXPRESS;Initial Catalog=SafeEats;Integrated Security=True;";

                using (SqlConnection conexao = new SqlConnection(conString))
                {
                    conexao.Open();

                    // Buscar o idCliente associado ao email fornecido
                    string buscarIdClienteSql = "SELECT idCliente FROM Cliente WHERE idUsuario = (SELECT idUsuario FROM Usuario WHERE email = @Email)";
                    SqlCommand cmdBuscarIdCliente = new SqlCommand(buscarIdClienteSql, conexao);
                    cmdBuscarIdCliente.Parameters.AddWithValue("@Email", emailLogado);
                    int idCliente = (int)cmdBuscarIdCliente.ExecuteScalar();

                    Console.WriteLine("Filtrar por status de pedido:");
                    Console.WriteLine("1 - Em Processamento");
                    Console.WriteLine("2 - Concluído");
                    Console.WriteLine("3 - Cancelado");
                    Console.WriteLine("0 - Mostrar todos");
                    Console.Write("\nEscolha uma opção: ");
                    int opcaoFiltro = int.Parse(Console.ReadLine());

                    // Consultar os pedidos do cliente de acordo com o filtro selecionado
                    string consultarPedidosSql = @"SELECT codPedido, dtPedido, statusPedido
                            FROM Pedido
                            WHERE idCliente = @IdCliente";

                    if (opcaoFiltro == 1)
                    {
                        consultarPedidosSql += " AND statusPedido = 'Em Processamento'";
                    }
                    else if (opcaoFiltro == 2)
                    {
                        consultarPedidosSql += " AND statusPedido = 'Concluído'";
                    }
                    else if (opcaoFiltro == 3)
                    {
                        consultarPedidosSql += " AND statusPedido = 'Cancelado'";
                    }

                    SqlCommand cmdConsultarPedidos = new SqlCommand(consultarPedidosSql, conexao);
                    cmdConsultarPedidos.Parameters.AddWithValue("@IdCliente", idCliente);

                    SqlDataReader reader = cmdConsultarPedidos.ExecuteReader();

                    if (reader.HasRows)
                    {
                        Console.WriteLine("\nMeus Pedidos:\n");

                        while (reader.Read())
                        {
                            int codPedido = reader.GetInt32(0);
                            DateTime dtPedido = reader.GetDateTime(1).Date;
                            string statusPedido = reader.GetString(2);

                            Console.WriteLine($"Código do Pedido: {codPedido}");
                            Console.WriteLine($"Data do Pedido: {dtPedido.ToString("dd/MM/yyyy")}");
                            Console.WriteLine($"Status do Pedido: {statusPedido}\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Não há pedidos realizados.");
                    }
                }
            }

            // Sessão de administrador do sistema
            public void menuPrincipalAdmin(string emailLogado)
            {
                TelaLogin tl = new TelaLogin();
                menuPrincipalMensagem();

                Console.WriteLine("\n[GERENCIAMENTO] Menu Global\n");
                Console.WriteLine("1 - Gerenciar produtos");
                Console.WriteLine("2 - Gerenciar fornecedores");
                Console.WriteLine("3 - Gerenciar vendas");
                Console.WriteLine("4 - Gerenciar estoque");
                Console.WriteLine("0 - Sair");
                Console.Write("\nDigite a opção desejada: ");
                string op = Console.ReadLine();

                switch (op)
                {
                    case "1":
                        GerenciarProdutos(emailLogado);
                        break;
                    case "2":
                        GerenciarFornecedores(emailLogado);
                        break;
                    case "3":
                        GerenciarVendas(emailLogado);
                        break;
                    case "4":
                        ListarProdutosEmEstoque(emailLogado);
                        break;
                    default:
                        comandoInexistente();
                        menuPrincipalAdmin(emailLogado);
                        break;
                }
            }

            private void GerenciarProdutos(string emailLogado)
            {
                string conString = @"Data Source=DESKTOP-UUDL5AC\SQLEXPRESS;Initial Catalog=SafeEats;Integrated Security=True;";

                while (true)
                {
                    Console.Clear();
                    mensagemProdutos();

                    Console.WriteLine("\n[GERENCIAMENTO] Produtos\n");
                    Console.WriteLine("1 - Adicionar um produto");
                    Console.WriteLine("2 - Excluir um produto");
                    Console.WriteLine("3 - Alterar um produto");
                    Console.WriteLine("4 - Sair");
                    Console.Write("\nEscolha uma opção: ");
                    string opcao = Console.ReadLine();

                    switch (opcao)
                    {
                        case "1":
                            AdicionarProduto(emailLogado);
                            break;
                        case "2":
                            ExcluirProduto(emailLogado);
                            break;
                        case "3":
                            AlterarProduto(emailLogado);
                            break;
                        case "4":
                            Console.Clear();
                            menuPrincipalAdmin(emailLogado);
                            break;
                        default:
                            Console.WriteLine("Opção inválida. Tente novamente.");
                            break;
                    }
                }
            }

            private void AdicionarProduto(string emailLogado)
            {
                string conString = @"Data Source=DESKTOP-UUDL5AC\SQLEXPRESS;Initial Catalog=SafeEats;Integrated Security=True;";

                Console.Clear();
                mensagemProdutos();

                Console.Write("Nome: ");
                string nome = Console.ReadLine();
                nome = Console.ReadLine();
                Console.Write("Descrição: ");
                string descricao = Console.ReadLine();
                descricao = Console.ReadLine();
                Console.Write("Preço: ");
                decimal preco = decimal.Parse(Console.ReadLine());
                Console.Write("Quantidade: ");
                int quantidade = Int32.Parse(Console.ReadLine());

                using (SqlConnection conexao = new SqlConnection(conString))
                {
                    conexao.Open();
                    string inserirProdutoSql = "INSERT INTO Produto (nome, descricao, preco, quantidade) VALUES (@nome, @descricao, @preco, @quantidade)";
                    SqlCommand cmdInserirProduto = new SqlCommand(inserirProdutoSql, conexao);
                    cmdInserirProduto.Parameters.AddWithValue("@nome", nome);
                    cmdInserirProduto.Parameters.AddWithValue("@descricao", descricao);
                    cmdInserirProduto.Parameters.AddWithValue("@preco", preco);
                    cmdInserirProduto.Parameters.AddWithValue("@quantidade", quantidade);
                    cmdInserirProduto.ExecuteNonQuery();
                }

                Console.Clear();
                mensagemProdutos();
                Console.WriteLine("Produto adicionado com sucesso!");
                Thread.Sleep(1500);
                Console.Clear();
                GerenciarProdutos(emailLogado);
            }

            private void ExcluirProduto(string emailLogado)
            {
                string conString = @"Data Source=DESKTOP-UUDL5AC\SQLEXPRESS;Initial Catalog=SafeEats;Integrated Security=True;";

                Console.Clear();
                mensagemProdutos();

                using (SqlConnection conexao = new SqlConnection(conString))
                {
                    conexao.Open();
                    string consulta = "SELECT idProduto, nome, descricao, preco FROM Produto";
                    SqlCommand cmd = new SqlCommand(consulta, conexao);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["idProduto"]} | Nome: {reader["nome"]} | Preço: R${reader["preco"]}");
                    }

                    reader.Close();
                }

                Console.Write("\n\n[GERENCIAMENTO] Digite o ID a excluir: ");
                int idProduto = int.Parse(Console.ReadLine());

                Console.Clear();
                mensagemProdutos();

                Console.Write("[GERENCIAMENTO] Confirmar ação!\n\n1 - Sim\n2 - Não\n\nDigite a opção: ");
                string op = Console.ReadLine();

                if(op == "1" || op.ToLower() == "s" || op.ToLower() == "sim")
                {
                    using (SqlConnection conexao = new SqlConnection(conString))
                    {
                        conexao.Open();
                        string excluirProdutoSql = "DELETE FROM Produto WHERE idProduto = @idProduto";
                        SqlCommand cmdExcluirProduto = new SqlCommand(excluirProdutoSql, conexao);
                        cmdExcluirProduto.Parameters.AddWithValue("@idProduto", idProduto);
                        cmdExcluirProduto.ExecuteNonQuery();
                    }
                    Console.Clear();
                    mensagemProdutos();
                    Console.WriteLine("[GERENCIAMENTO] Produto excluído com sucesso!");
                    Thread.Sleep(1500);
                    Console.Clear();
                    GerenciarProdutos(emailLogado);
                }
                else if (op == "2" || op.ToLower() == "n" || op.ToLower() == "não" || op.ToLower() == "nao")
                {
                    Console.Clear();
                    mensagemProdutos();
                    Console.WriteLine("[GERENCIAMENTO] Exclusão de produto cancelada.");
                    Thread.Sleep(1500);
                    Console.Clear();
                    GerenciarProdutos(emailLogado);
                }
            }

            private void AlterarProduto(string emailLogado)
            {
                string conString = @"Data Source=DESKTOP-UUDL5AC\SQLEXPRESS;Initial Catalog=SafeEats;Integrated Security=True;";

                Console.Clear();
                mensagemProdutos();

                using (SqlConnection conexao = new SqlConnection(conString))
                {
                    conexao.Open();
                    string consulta = "SELECT idProduto, nome, descricao, preco, quantidade FROM Produto";
                    SqlCommand cmd = new SqlCommand(consulta, conexao);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["idProduto"]} | Nome: {reader["nome"]} | Preço: R${reader["preco"]} | Quantidade: {reader["quantidade"]}");
                    }

                    reader.Close();
                }

                Console.Write("\n\n[GERENCIAMENTO] Digite o ID do produto a alterar: ");
                if (!int.TryParse(Console.ReadLine(), out int idProduto))
                {
                    Console.Clear();
                    mensagemProdutos();
                    Console.WriteLine("Produto não alterado.\nMotivo: ID inválido.");
                    Thread.Sleep(1500);
                    Console.Clear();
                    GerenciarProdutos(emailLogado);
                    return;
                }

                // Verifica se o ID do produto existe no banco de dados
                using (SqlConnection conexao = new SqlConnection(conString))
                {
                    conexao.Open();
                    string verificarIdSql = "SELECT COUNT(*) FROM Produto WHERE idProduto = @idProduto";
                    SqlCommand cmdVerificarId = new SqlCommand(verificarIdSql, conexao);
                    cmdVerificarId.Parameters.AddWithValue("@idProduto", idProduto);
                    int count = (int)cmdVerificarId.ExecuteScalar();

                    if (count == 0)
                    {
                        Console.Clear();
                        mensagemProdutos();
                        Console.WriteLine("Produto não alterado.\nMotivo: ID do produto inexistente.");
                        Thread.Sleep(1500);
                        Console.Clear();
                        GerenciarProdutos(emailLogado);
                        return;
                    }
                }

                Console.Clear();
                mensagemProdutos();
                Console.WriteLine("[GERENCIAMENTO] O que você deseja alterar?\n");
                Console.WriteLine("1 - Preço");
                Console.WriteLine("2 - Quantidade\n");
                Console.Write("Escolha uma opção: ");

                string opcao = Console.ReadLine();

                if (opcao == "1")
                {
                    Console.Clear();
                    mensagemProdutos();
                    using (SqlConnection conexao = new SqlConnection(conString))
                    {
                        conexao.Open();
                        string consulta = "SELECT preco FROM Produto WHERE idProduto = @idProduto";
                        SqlCommand cmd = new SqlCommand(consulta, conexao);
                        cmd.Parameters.AddWithValue("@idProduto", idProduto);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            Console.WriteLine($"[GERENCIAMENTO] Preço Antigo: R${reader["preco"]}");
                        }

                        reader.Close();
                    }
                    Console.Write("[GERENCIAMENTO] Novo Preço: R$");
                    string inputPreco = Console.ReadLine();

                    // Substitui o ponto por vírgula para suportar a cultura brasileira
                    inputPreco = inputPreco.Replace('.', ',');

                    if (!decimal.TryParse(inputPreco, out decimal novoPreco))
                    {
                        Console.Clear();
                        mensagemProdutos();
                        Console.WriteLine("Produto não alterado.\nMotivo: Preço inválido.");
                        Thread.Sleep(1500);
                        Console.Clear();
                        GerenciarProdutos(emailLogado);
                        return;
                    }

                    using (SqlConnection conexao = new SqlConnection(conString))
                    {
                        conexao.Open();
                        string alterarPrecoSql = "UPDATE Produto SET preco = @novoPreco WHERE idProduto = @idProduto";
                        SqlCommand cmdAlterarPreco = new SqlCommand(alterarPrecoSql, conexao);
                        cmdAlterarPreco.Parameters.AddWithValue("@novoPreco", novoPreco);
                        cmdAlterarPreco.Parameters.AddWithValue("@idProduto", idProduto);
                        cmdAlterarPreco.ExecuteNonQuery();
                    }
                    Console.Clear();
                    mensagemProdutos();
                    Console.WriteLine("Preço do produto alterado com sucesso!");
                }
                else if (opcao == "2")
                {
                    Console.Clear();
                    mensagemProdutos();
                    Console.Write("[GERENCIAMENTO] Nova Quantidade: ");
                    if (!int.TryParse(Console.ReadLine(), out int novaQuantidade))
                    {
                        Console.Clear();
                        mensagemProdutos();
                        Console.WriteLine("Produto não alterado.\nMotivo: Quantidade inválida.");
                        Thread.Sleep(1500);
                        Console.Clear();
                        GerenciarProdutos(emailLogado);
                        return;
                    }

                    using (SqlConnection conexao = new SqlConnection(conString))
                    {
                        conexao.Open();
                        string alterarQuantidadeSql = "UPDATE Produto SET quantidade = @novaQuantidade WHERE idProduto = @idProduto";
                        SqlCommand cmdAlterarQuantidade = new SqlCommand(alterarQuantidadeSql, conexao);
                        cmdAlterarQuantidade.Parameters.AddWithValue("@novaQuantidade", novaQuantidade);
                        cmdAlterarQuantidade.Parameters.AddWithValue("@idProduto", idProduto);
                        cmdAlterarQuantidade.ExecuteNonQuery();
                    }
                    Console.Clear();
                    mensagemProdutos();
                    Console.WriteLine("Quantidade do produto alterada com sucesso!");
                }
                else
                {
                    Console.Clear();
                    mensagemProdutos();
                    Console.WriteLine("Opção inválida. Nenhuma alteração foi realizada.");
                }

                Thread.Sleep(1500);
                Console.Clear();
                GerenciarProdutos(emailLogado);
            }

            public void ListarProdutosEmEstoque(string emailLogado)
            {
                string conString = @"Data Source=DESKTOP-UUDL5AC\SQLEXPRESS;Initial Catalog=SafeEats;Integrated Security=True;";

                Console.Clear();
                mensagemEstoque();

                using (SqlConnection conexao = new SqlConnection(conString))
                {
                    conexao.Open();

                    string consulta = "SELECT idProduto, nome, descricao, preco, quantidade FROM Produto";
                    SqlCommand cmd = new SqlCommand(consulta, conexao);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        int idProduto = reader.GetInt32(0);
                        string nome = reader.GetString(1);
                        string descricao = reader.GetString(2);
                        decimal preco = reader.GetDecimal(3);
                        int quantidade = reader.GetInt32(4);

                        Console.WriteLine($"ID: {idProduto} | Nome: {nome} | Descrição: {descricao} | Preço: {preco:C2} | Quantidade: {quantidade}");
                    }

                    reader.Close();
                }

                Console.WriteLine("\nPressione qualquer tecla para voltar ao menu principal...");
                Console.ReadKey();
                Console.Clear();
                menuPrincipalAdmin(emailLogado);
            }

            private void GerenciarFornecedores(string emailLogado)
            {
                string conString = @"Data Source=DESKTOP-UUDL5AC\SQLEXPRESS;Initial Catalog=SafeEats;Integrated Security=True;";

                while (true)
                {
                    Console.Clear();
                    mensagemFornecedor();
                    Console.WriteLine("\n[GERENCIAMENTO] Fornecedores\n");
                    Console.WriteLine("1 - Adicionar um fornecedor");
                    Console.WriteLine("2 - Excluir um fornecedor");
                    Console.WriteLine("3 - Alterar um fornecedor");
                    Console.WriteLine("4 - Listar fornecedores");
                    Console.WriteLine("0 - Sair");
                    Console.Write("\nEscolha uma opção: ");
                    string opcao = Console.ReadLine();

                    switch (opcao)
                    {
                        case "1":
                            AdicionarFornecedor(emailLogado, conString);
                            break;
                        case "2":
                            ExcluirFornecedor(emailLogado, conString);
                            break;
                        case "3":
                            AlterarFornecedor(emailLogado, conString);
                            break;
                        case "4":
                            ListarFornecedores(emailLogado, conString);
                            break;
                        case "0":
                            Console.Clear();
                            menuPrincipalAdmin(emailLogado);
                            break;
                        default:
                            Console.WriteLine("Opção inválida. Tente novamente.");
                            break;
                    }
                }
            }

            void AdicionarFornecedor(string emailLogado, string conString)
            {
                string nomeCompleto, cnpj, email, senhaUsuario, confirmarSenha, tipoProduto, razaoSocial, nomeFantasia;

                using (SqlConnection conexao = new SqlConnection(conString))
                {
                    conexao.Open();

                    Console.Clear();
                    mensagemFornecedor();

                    while (true)
                    {
                        Console.Write("Digite o nome completo: ");
                        nomeCompleto = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(nomeCompleto))
                        {
                            Console.Clear();
                            //mensagemCadastro();
                            Console.WriteLine("O nome não pode estar vazio. Por favor, digite novamente.");
                            Thread.Sleep(1500);
                            Console.Clear();
                            //mensagemCadastro();
                            continue; // Retorna ao início do loop para solicitar o nome novamente
                        }
                        else
                        {
                            break; // Sai do loop se o nome não estiver vazio
                        }
                    }

                    while (true)
                    {
                        Console.Write("Digite o CNPJ: ");
                        cnpj = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(cnpj))
                        {
                            Console.Clear();
                            //mensagemCadastro();
                            Console.WriteLine("O CNPJ não pode estar vazio. Por favor, digite novamente.");
                            Thread.Sleep(1500);
                            Console.Clear();
                            //mensagemCadastro();
                            Console.WriteLine($"Nome completo: {nomeCompleto}");
                            continue; // Retorna ao início do loop para solicitar o CNPJ novamente
                        }
                        else
                        {
                            break; // Sai do loop se o CNPJ não estiver vazio
                        }
                    }

                    while (true)
                    {
                        Console.Write("Digite o e-mail: ");
                        email = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(email))
                        {
                            Console.Clear();
                            //mensagemCadastro();
                            Console.WriteLine("O e-mail não pode estar vazio. Por favor, digite novamente.");
                            Thread.Sleep(1500);
                            Console.Clear();
                            //mensagemCadastro();
                            Console.WriteLine($"Nome completo: {nomeCompleto}\nCNPJ: {cnpj}");
                            continue; // Retorna ao início do loop para solicitar o e-mail novamente
                        }

                        // Consulta SQL para verificar se o e-mail já existe
                        string verificarEmailSql = "SELECT COUNT(*) FROM Usuario WHERE email = @Email";
                        SqlCommand cmdVerificarEmail = new SqlCommand(verificarEmailSql, conexao);
                        cmdVerificarEmail.Parameters.AddWithValue("@Email", email);
                        int count = (int)cmdVerificarEmail.ExecuteScalar();

                        // Verifica se o e-mail já existe
                        if (count > 0)
                        {
                            Console.Clear();
                            //mensagemCadastro();
                            Console.WriteLine("O e-mail já está em uso! Por favor, escolha outro.");
                            Thread.Sleep(1500);
                            Console.Clear();
                            //mensagemCadastro();
                            Console.WriteLine($"Nome completo: {nomeCompleto}\nCNPJ: {cnpj}");
                            continue; // Retorna ao início do loop para solicitar o e-mail novamente
                        }
                        else
                        {
                            break; // Sai do loop se o e-mail não existir
                        }
                    }


                    Console.Clear();
                    mensagemFornecedor();

                    while (true)
                    {
                        Console.Write("Digite a razão social: ");
                        razaoSocial = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(razaoSocial))
                        {
                            Console.Clear();
                            //mensagemCadastro();
                            Console.WriteLine("A razão social não pode estar vazia. Por favor, digite novamente.");
                            Thread.Sleep(1500);
                            Console.Clear();
                            //mensagemCadastro();
                            continue; // Retorna ao início do loop para solicitar a razão social novamente
                        }
                        else
                        {
                            break; // Sai do loop se a razão social não estiver vazia
                        }
                    }

                    while (true)
                    {
                        Console.Write("Digite o nome fantasia: ");
                        nomeFantasia = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(nomeFantasia))
                        {
                            Console.Clear();
                            //mensagemCadastro();
                            Console.WriteLine("O nome fantasia não pode estar vazio. Por favor, digite novamente.");
                            Thread.Sleep(1500);
                            Console.Clear();
                            //mensagemCadastro();
                            continue; // Retorna ao início do loop para solicitar o nome fantasia novamente
                        }
                        else
                        {
                            break; // Sai do loop se o nome fantasia não estiver vazio
                        }
                    }

                    while (true)
                    {
                        Console.Write("Digite o tipo de produto: ");
                        tipoProduto = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(tipoProduto))
                        {
                            Console.Clear();
                            //mensagemCadastro();
                            Console.WriteLine("O tipo de produto não pode estar vazio. Por favor, digite novamente.");
                            Thread.Sleep(1500);
                            Console.Clear();
                            //mensagemCadastro();
                            continue; // Retorna ao início do loop para solicitar o tipo de produto novamente
                        }
                        else
                        {
                            break; // Sai do loop se o tipo de produto não estiver vazio
                        }
                    }

                    Console.Clear();
                    mensagemFornecedor();

                    while (true)
                    {
                        Console.Write("Digite a senha: ");
                        senhaUsuario = "";
                        mascaraSenha(ref senhaUsuario);

                        if (string.IsNullOrWhiteSpace(senhaUsuario))
                        {
                            Console.Clear();
                            //mensagemCadastro();
                            Console.WriteLine("A senha não pode estar vazia. Por favor, digite novamente.");
                            Thread.Sleep(1500);
                            Console.Clear();
                            //mensagemCadastro();
                            continue; // Retorna ao início do loop para solicitar a senha novamente
                        }

                        Console.Write("Confirme sua senha: ");
                        confirmarSenha = "";
                        mascaraSenha(ref confirmarSenha);

                        if (senhaUsuario == confirmarSenha)
                        {
                            // Se as senhas conferem, sai do loop
                            break;
                        }
                        else
                        {
                            Console.Clear();
                            //mensagemCadastro();
                            Console.WriteLine("As senhas não conferem. Por favor, tente novamente.");
                            Thread.Sleep(1500);
                            Console.Clear();
                            //mensagemCadastro();
                        }
                    }

                    // Se as senhas conferem e nenhum campo obrigatório está vazio, insere o novo fornecedor na tabela Usuario
                    string inserirUsuarioSql = "INSERT INTO Usuario (nome, email, senha) OUTPUT INSERTED.idUsuario VALUES (@nome, @Email, @senha)";
                    SqlCommand cmdInserirUsuario = new SqlCommand(inserirUsuarioSql, conexao);
                    cmdInserirUsuario.Parameters.AddWithValue("@nome", nomeCompleto);
                    cmdInserirUsuario.Parameters.AddWithValue("@Email", email);
                    cmdInserirUsuario.Parameters.AddWithValue("@senha", senhaUsuario);

                    int idUsuario = (int)cmdInserirUsuario.ExecuteScalar();

                    // Insere o novo fornecedor na tabela Fornecedor com a chave estrangeira idUsuario
                    string inserirFornecedorSql = "INSERT INTO Fornecedor (idUsuario, cnpj, tipoProduto, razaoSocial, nomeFantasia) VALUES (@idUsuario, @cnpj, @tipoProduto, @razaoSocial, @nomeFantasia)";
                    SqlCommand cmdInserirFornecedor = new SqlCommand(inserirFornecedorSql, conexao);
                    cmdInserirFornecedor.Parameters.AddWithValue("@idUsuario", idUsuario);
                    cmdInserirFornecedor.Parameters.AddWithValue("@cnpj", cnpj);
                    cmdInserirFornecedor.Parameters.AddWithValue("@tipoProduto", tipoProduto);
                    cmdInserirFornecedor.Parameters.AddWithValue("@razaoSocial", razaoSocial);
                    cmdInserirFornecedor.Parameters.AddWithValue("@nomeFantasia", nomeFantasia);
                    cmdInserirFornecedor.ExecuteNonQuery();

                    Console.Clear();
                    mensagemFornecedor();
                    Console.WriteLine("Cadastro realizado com sucesso");
                    Thread.Sleep(1500);
                    Console.Clear();
                    GerenciarFornecedores(emailLogado);
                }
            }

            void ExcluirFornecedor(string emailLogado, string conString)
            {
                Console.Clear();
                mensagemFornecedor();

                using (SqlConnection conexao = new SqlConnection(conString))
                {
                    conexao.Open();
                    string consulta = "SELECT idFornecedor, nomeFantasia FROM Fornecedor";
                    SqlCommand cmd = new SqlCommand(consulta, conexao);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["idFornecedor"]} | Nome Fantasia: {reader["nomeFantasia"]}");
                    }

                    reader.Close();
                }

                Console.Write("\n\n[GERENCIAMENTO] Digite o ID do fornecedor a excluir: ");
                int idFornecedor = int.Parse(Console.ReadLine());

                Console.Clear();
                mensagemFornecedor();

                Console.Write("[GERENCIAMENTO] Confirmar ação!\n\n1 - Sim\n2 - Não\n\nDigite a opção: ");
                string op = Console.ReadLine();

                if (op == "1" || op.ToLower() == "s" || op.ToLower() == "sim")
                {
                    using (SqlConnection conexao = new SqlConnection(conString))
                    {
                        conexao.Open();
                        string excluirFornecedorSql = "DELETE FROM Fornecedor WHERE idFornecedor = @idFornecedor";
                        SqlCommand cmdExcluirFornecedor = new SqlCommand(excluirFornecedorSql, conexao);
                        cmdExcluirFornecedor.Parameters.AddWithValue("@idFornecedor", idFornecedor);
                        cmdExcluirFornecedor.ExecuteNonQuery();
                    }
                    Console.Clear();
                    mensagemFornecedor();
                    Console.WriteLine("[GERENCIAMENTO] Fornecedor excluído com sucesso!");
                    Thread.Sleep(1500);
                    Console.Clear();
                    GerenciarFornecedores(emailLogado);
                }
                else if (op == "2" || op.ToLower() == "n" || op.ToLower() == "não" || op.ToLower() == "nao")
                {
                    Console.Clear();
                    mensagemFornecedor();
                    Console.WriteLine("[GERENCIAMENTO] Exclusão de fornecedor cancelada.");
                    Thread.Sleep(1500);
                    Console.Clear();
                    GerenciarFornecedores(emailLogado);
                }
            }

            private void AlterarFornecedor(string emailLogado, string conString)
            {
                Console.Clear();
                mensagemFornecedor();

                using (SqlConnection conexao = new SqlConnection(conString))
                {
                    conexao.Open();
                    string consulta = "SELECT idFornecedor, razaoSocial, nomeFantasia, cnpj FROM Fornecedor";
                    SqlCommand cmd = new SqlCommand(consulta, conexao);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["idFornecedor"]} | Razão Social: {reader["razaoSocial"]} | Nome Fantasia: {reader["nomeFantasia"]} | CNPJ: {reader["cnpj"]}");
                    }

                    reader.Close();
                }

                Console.Write("\n\n[GERENCIAMENTO] Digite o ID do fornecedor a alterar: ");
                if (!int.TryParse(Console.ReadLine(), out int idFornecedor))
                {
                    Console.Clear();
                    mensagemFornecedor();
                    Console.WriteLine("Fornecedor não alterado.\nMotivo: ID inválido.");
                    Thread.Sleep(1500);
                    Console.Clear();
                    GerenciarFornecedores(emailLogado);
                    return;
                }

                // Verifica se o ID do fornecedor existe no banco de dados
                using (SqlConnection conexao = new SqlConnection(conString))
                {
                    conexao.Open();
                    string verificarIdSql = "SELECT COUNT(*) FROM Fornecedor WHERE idFornecedor = @idFornecedor";
                    SqlCommand cmdVerificarId = new SqlCommand(verificarIdSql, conexao);
                    cmdVerificarId.Parameters.AddWithValue("@idFornecedor", idFornecedor);
                    int count = (int)cmdVerificarId.ExecuteScalar();

                    if (count == 0)
                    {
                        Console.Clear();
                        mensagemFornecedor();
                        Console.WriteLine("Fornecedor não alterado.\nMotivo: ID do fornecedor inexistente.");
                        Thread.Sleep(1500);
                        Console.Clear();
                        GerenciarFornecedores(emailLogado);
                        return;
                    }
                }

                Console.Clear();
                mensagemFornecedor();
                Console.WriteLine("[GERENCIAMENTO] O que você deseja alterar?\n");
                Console.WriteLine("1 - CNPJ");
                Console.WriteLine("2 - Nome Fantasia");
                Console.WriteLine("3 - Razão Social\n");
                Console.Write("Escolha uma opção: ");

                string opcao = Console.ReadLine();

                if (opcao == "1")
                {
                    Console.Clear();
                    mensagemFornecedor();
                    Console.Write("[GERENCIAMENTO] Novo CNPJ: ");
                    string novoCnpj = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(novoCnpj))
                    {
                        Console.Clear();
                        mensagemFornecedor();
                        Console.WriteLine("Fornecedor não alterado.\nMotivo: CNPJ inválido.");
                        Thread.Sleep(1500);
                        Console.Clear();
                        GerenciarFornecedores(emailLogado);
                        return;
                    }

                    using (SqlConnection conexao = new SqlConnection(conString))
                    {
                        conexao.Open();
                        string alterarCnpjSql = "UPDATE Fornecedor SET cnpj = @novoCnpj WHERE idFornecedor = @idFornecedor";
                        SqlCommand cmdAlterarCnpj = new SqlCommand(alterarCnpjSql, conexao);
                        cmdAlterarCnpj.Parameters.AddWithValue("@novoCnpj", novoCnpj);
                        cmdAlterarCnpj.Parameters.AddWithValue("@idFornecedor", idFornecedor);
                        cmdAlterarCnpj.ExecuteNonQuery();
                    }
                    Console.Clear();
                    mensagemFornecedor();
                    Console.WriteLine("CNPJ do fornecedor alterado com sucesso!");
                }
                else if (opcao == "2")
                {
                    Console.Clear();
                    mensagemFornecedor();
                    Console.Write("[GERENCIAMENTO] Novo Nome Fantasia: ");
                    string novoNomeFantasia = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(novoNomeFantasia))
                    {
                        Console.Clear();
                        mensagemFornecedor();
                        Console.WriteLine("Fornecedor não alterado.\nMotivo: Nome Fantasia inválido.");
                        Thread.Sleep(1500);
                        Console.Clear();
                        GerenciarFornecedores(emailLogado);
                        return;
                    }

                    using (SqlConnection conexao = new SqlConnection(conString))
                    {
                        conexao.Open();
                        string alterarNomeFantasiaSql = "UPDATE Fornecedor SET nomeFantasia = @novoNomeFantasia WHERE idFornecedor = @idFornecedor";
                        SqlCommand cmdAlterarNomeFantasia = new SqlCommand(alterarNomeFantasiaSql, conexao);
                        cmdAlterarNomeFantasia.Parameters.AddWithValue("@novoNomeFantasia", novoNomeFantasia);
                        cmdAlterarNomeFantasia.Parameters.AddWithValue("@idFornecedor", idFornecedor);
                        cmdAlterarNomeFantasia.ExecuteNonQuery();
                    }
                    Console.Clear();
                    mensagemFornecedor();
                    Console.WriteLine("Nome Fantasia do fornecedor alterado com sucesso!");
                }
                else if (opcao == "3")
                {
                    Console.Clear();
                    mensagemFornecedor();
                    Console.Write("[GERENCIAMENTO] Nova Razão Social: ");
                    string novaRazaoSocial = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(novaRazaoSocial))
                    {
                        Console.Clear();
                        mensagemFornecedor();
                        Console.WriteLine("Fornecedor não alterado.\nMotivo: Razão Social inválida.");
                        Thread.Sleep(1500);
                        Console.Clear();
                        GerenciarFornecedores(emailLogado);
                        return;
                    }

                    using (SqlConnection conexao = new SqlConnection(conString))
                    {
                        conexao.Open();
                        string alterarRazaoSocialSql = "UPDATE Fornecedor SET razaoSocial = @novaRazaoSocial WHERE idFornecedor = @idFornecedor";
                        SqlCommand cmdAlterarRazaoSocial = new SqlCommand(alterarRazaoSocialSql, conexao);
                        cmdAlterarRazaoSocial.Parameters.AddWithValue("@novaRazaoSocial", novaRazaoSocial);
                        cmdAlterarRazaoSocial.Parameters.AddWithValue("@idFornecedor", idFornecedor);
                        cmdAlterarRazaoSocial.ExecuteNonQuery();
                    }
                    Console.Clear();
                    mensagemFornecedor();
                    Console.WriteLine("Razão Social do fornecedor alterada com sucesso!");
                }
                else
                {
                    Console.Clear();
                    mensagemFornecedor();
                    Console.WriteLine("Opção inválida. Nenhuma alteração foi realizada.");
                }

                Thread.Sleep(1500);
                Console.Clear();
                GerenciarFornecedores(emailLogado);
            }

            public void ListarFornecedores(string emailLogado, string conString)
            {
                Console.Clear();
                mensagemFornecedor();

                using (SqlConnection conexao = new SqlConnection(conString))
                {
                    conexao.Open();

                    string consulta = "SELECT idFornecedor, nomeFantasia, razaoSocial, cnpj FROM Fornecedor";
                    SqlCommand cmd = new SqlCommand(consulta, conexao);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        int idFornecedor = reader.GetInt32(0);
                        string nomeFantasia = reader.GetString(1);
                        string razaoSocial = reader.GetString(2);
                        string cnpj = reader.GetString(3);

                        Console.WriteLine($"ID: {idFornecedor} | Nome Fantasia: {nomeFantasia} | Razão Social: {razaoSocial} | CNPJ: {cnpj}");
                    }

                    reader.Close();
                }

                Console.WriteLine("\nPressione qualquer tecla para voltar ao menu principal...");
                Console.ReadKey();
                Console.Clear();
                menuPrincipalAdmin(emailLogado);
            }

            public void GerenciarVendas(string emailLogado)
            {
                while (true)
                {
                    Console.Clear();
                    mensagemProdutos();

                    Console.WriteLine("\n[GERENCIAMENTO] Vendas\n");
                    Console.WriteLine("1 - Alterar uma venda");
                    Console.WriteLine("2 - Excluir uma venda");
                    Console.WriteLine("3 - Listar vendas");
                    Console.WriteLine("4 - Sair");
                    Console.Write("\nEscolha uma opção: ");
                    string opcao = Console.ReadLine();

                    switch (opcao)
                    {
                        case "1":
                            AlterarPedido(emailLogado);
                            break;
                        case "2":
                            ExcluirProduto(emailLogado);
                            break;
                        case "3":
                            AlterarProduto(emailLogado);
                            break;
                        case "4":
                            Console.Clear();
                            menuPrincipalAdmin(emailLogado);
                            break;
                        default:
                            Console.WriteLine("Opção inválida. Tente novamente.");
                            break;
                    }
                }
            }

            public void AlterarPedido(string emailLogado)
            {
                string conString = @"Data Source=DESKTOP-UUDL5AC\SQLEXPRESS;Initial Catalog=SafeEats;Integrated Security=True;";

                Console.Clear();
                mensagemVendas();

                using (SqlConnection conexao = new SqlConnection(conString))
                {
                    conexao.Open();
                    string consulta = "SELECT codPedido, idFormaPagamento, idFormaEnvio, dtPedido FROM Pedido";
                    SqlCommand cmd = new SqlCommand(consulta, conexao);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        int idFormaPagamento = reader.GetInt32(reader.GetOrdinal("idFormaPagamento"));
                        int idFormaEnvio = reader.GetInt32(reader.GetOrdinal("idFormaEnvio"));

                        string formaPagamentoFormatada;
                        switch (idFormaPagamento)
                        {
                            case 1:
                                formaPagamentoFormatada = "Crédito";
                                break;
                            case 2:
                                formaPagamentoFormatada = "Débito";
                                break;
                            case 3:
                                formaPagamentoFormatada = "PIX";
                                break;
                            default:
                                formaPagamentoFormatada = "Desconhecida";
                                break;
                        }

                        string formaEnvioFormatada;
                        switch (idFormaEnvio)
                        {
                            case 1:
                                formaEnvioFormatada = "Correios";
                                break;
                            case 2:
                                formaEnvioFormatada = "Transportadora";
                                break;
                            default:
                                formaEnvioFormatada = "Desconhecida";
                                break;
                        }

                        DateTime dtPedido = reader.GetDateTime(reader.GetOrdinal("dtPedido"));
                        string dataPedidoFormatada = dtPedido.ToShortDateString();

                        Console.WriteLine($"Código Pedido: {reader["codPedido"]} | Pagamento: {formaPagamentoFormatada} | Envio: {formaEnvioFormatada} | Data do Pedido: {dataPedidoFormatada}");
                    }

                    reader.Close();
                }

                Console.Write("\n\n[GERENCIAMENTO] Digite o ID do produto a alterar: ");
                if (!int.TryParse(Console.ReadLine(), out int codPedido))
                {
                    Console.Clear();
                    mensagemProdutos();
                    Console.WriteLine("Pedido não alterado.\nMotivo: ID inválido.");
                    Thread.Sleep(1500);
                    Console.Clear();
                    GerenciarProdutos(emailLogado);
                    return;
                }

                // Verifica se o ID do produto existe no banco de dados
                using (SqlConnection conexao = new SqlConnection(conString))
                {
                    conexao.Open();
                    string verificarIdSql = "SELECT COUNT(*) FROM Pedido WHERE codPedido = @codPedido";
                    SqlCommand cmdVerificarId = new SqlCommand(verificarIdSql, conexao);
                    cmdVerificarId.Parameters.AddWithValue("@codPedido", codPedido);
                    int count = (int)cmdVerificarId.ExecuteScalar();

                    if (count == 0)
                    {
                        Console.Clear();
                        mensagemProdutos();
                        Console.WriteLine("Pedido não alterado.\nMotivo: ID do pedido inexistente.");
                        Thread.Sleep(1500);
                        Console.Clear();
                        GerenciarVendas(emailLogado);
                        return;
                    }
                }

                Console.Clear();
                mensagemProdutos();
                Console.WriteLine("[GERENCIAMENTO] O que você deseja alterar?\n");
                Console.WriteLine("1 - Forma de Pagamento");
                Console.WriteLine("2 - Forma de Envio");
                Console.WriteLine("3 - Status do Pedido\n");
                Console.Write("Escolha uma opção: ");

                string opcao = Console.ReadLine();

                if (opcao == "1")
                {
                    Console.Clear();
                    mensagemProdutos();
                    using (SqlConnection conexao = new SqlConnection(conString))
                    {
                        conexao.Open();
                        string consulta = "SELECT idFormaPagamento FROM Pedido WHERE codPedido = @codPedido";
                        SqlCommand cmd = new SqlCommand(consulta, conexao);
                        cmd.Parameters.AddWithValue("@codPedido", codPedido);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            int idFormaPagamento = reader.GetInt32(reader.GetOrdinal("idFormaPagamento"));

                            string formaPagamentoFormatada;
                            switch (idFormaPagamento)
                            {
                                case 1:
                                    formaPagamentoFormatada = "Crédito";
                                    break;
                                case 2:
                                    formaPagamentoFormatada = "Débito";
                                    break;
                                case 3:
                                    formaPagamentoFormatada = "PIX";
                                    break;
                                default:
                                    formaPagamentoFormatada = "Desconhecida";
                                    break;
                            }

                            Console.WriteLine($"[GERENCIAMENTO] Forma de pagamento antiga: {formaPagamentoFormatada}");
                        }

                        reader.Close();
                    }

                    int novaFormaPagamento = 0;
                    bool formaPagamentoValida = false;

                    while (!formaPagamentoValida)
                    {
                        Console.WriteLine("[GERENCIAMENTO] Nova forma de pagamento");
                        Console.Write("\n1 - Crédito\n2 - Débito\n3 - PIX\n\nEscolha a opção(1, 2, 3): ");
                        string inputFormaPagamento = Console.ReadLine();

                        switch (inputFormaPagamento)
                        {
                            case "1":
                                novaFormaPagamento = 1;
                                formaPagamentoValida = true;
                                break;
                            case "2":
                                novaFormaPagamento = 2;
                                formaPagamentoValida = true;
                                break;
                            case "3":
                                novaFormaPagamento = 3;
                                formaPagamentoValida = true;
                                break;
                            default:
                                Console.Clear();
                                mensagemProdutos();
                                Console.WriteLine("Opção inválida. Tente novamente.");
                                break;
                        }
                    }

                    using (SqlConnection conexao = new SqlConnection(conString))
                    {
                        conexao.Open();
                        string alterarFormaPagamentoSql = "UPDATE Pedido SET idFormaPagamento = @novaFormaPagamento WHERE codPedido = @codPedido";
                        SqlCommand cmdAlterarFormaPagamento = new SqlCommand(alterarFormaPagamentoSql, conexao);
                        cmdAlterarFormaPagamento.Parameters.AddWithValue("@novaFormaPagamento", novaFormaPagamento);
                        cmdAlterarFormaPagamento.Parameters.AddWithValue("@codPedido", codPedido);
                        cmdAlterarFormaPagamento.ExecuteNonQuery();
                    }

                    Console.Clear();
                    mensagemProdutos();
                    Console.WriteLine("Preço do produto alterado com sucesso!");
                }
                else if (opcao == "2")
                {
                    Console.Clear();
                    mensagemProdutos();
                    using (SqlConnection conexao = new SqlConnection(conString))
                    {
                        conexao.Open();
                        string consulta = "SELECT idFormaEnvio FROM Pedido WHERE codPedido = @codPedido";
                        SqlCommand cmd = new SqlCommand(consulta, conexao);
                        cmd.Parameters.AddWithValue("@codPedido", codPedido);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            int idFormaEnvio = reader.GetInt32(reader.GetOrdinal("idFormaEnvio"));

                            string formaEnvioFormatada;
                            switch (idFormaEnvio)
                            {
                                case 1:
                                    formaEnvioFormatada = "Correios";
                                    break;
                                case 2:
                                    formaEnvioFormatada = "Transportadora";
                                    break;
                                default:
                                    formaEnvioFormatada = "Desconhecida";
                                    break;
                            }

                            Console.WriteLine($"[GERENCIAMENTO] Forma de envio antiga: {formaEnvioFormatada}");
                        }

                        reader.Close();
                    }

                    int novaFormaEnvio = 0;
                    bool formaEnvioValida = false;

                    while (!formaEnvioValida)
                    {
                        Console.WriteLine("[GERENCIAMENTO] Nova forma de envio");
                        Console.Write("\n1 - Correios\n2 - Transportadora\n\nEscolha a opção(1 ou 2): ");
                        string inputFormaPagamento = Console.ReadLine();

                        switch (inputFormaPagamento)
                        {
                            case "1":
                                novaFormaEnvio = 1;
                                formaEnvioValida = true;
                                break;
                            case "2":
                                novaFormaEnvio = 2;
                                formaEnvioValida = true;
                                break;
                            default:
                                Console.Clear();
                                mensagemProdutos();
                                Console.WriteLine("Opção inválida. Tente novamente.");
                                break;
                        }
                    }

                    using (SqlConnection conexao = new SqlConnection(conString))
                    {
                        conexao.Open();
                        string alterarFormaPagamentoSql = "UPDATE Pedido SET idFormaEnvio = @novaFormaEnvio WHERE codPedido = @codPedido";
                        SqlCommand cmdAlterarFormaPagamento = new SqlCommand(alterarFormaPagamentoSql, conexao);
                        cmdAlterarFormaPagamento.Parameters.AddWithValue("@novaFormaEnvio", novaFormaEnvio);
                        cmdAlterarFormaPagamento.Parameters.AddWithValue("@codPedido", codPedido);
                        cmdAlterarFormaPagamento.ExecuteNonQuery();
                    }

                    Console.Clear();
                    mensagemVendas();
                    Console.WriteLine("Forma de envio do pedido alterado com sucesso!");
                }
                /*else if (opcao == "3")
                {
                    Console.Clear();
                    mensagemProdutos();
                    Console.Write("[GERENCIAMENTO] Nova Quantidade: ");
                    if (!int.TryParse(Console.ReadLine(), out int novaQuantidade))
                    {
                        Console.Clear();
                        mensagemProdutos();
                        Console.WriteLine("Produto não alterado.\nMotivo: Quantidade inválida.");
                        Thread.Sleep(1500);
                        Console.Clear();
                        GerenciarProdutos(emailLogado);
                        return;
                    }

                    using (SqlConnection conexao = new SqlConnection(conString))
                    {
                        conexao.Open();
                        string alterarQuantidadeSql = "UPDATE Produto SET quantidade = @novaQuantidade WHERE idProduto = @idProduto";
                        SqlCommand cmdAlterarQuantidade = new SqlCommand(alterarQuantidadeSql, conexao);
                        cmdAlterarQuantidade.Parameters.AddWithValue("@novaQuantidade", novaQuantidade);
                        cmdAlterarQuantidade.Parameters.AddWithValue("@idProduto", idProduto);
                        cmdAlterarQuantidade.ExecuteNonQuery();
                    }
                    Console.Clear();
                    mensagemProdutos();
                    Console.WriteLine("Quantidade do produto alterada com sucesso!");
                }*/
                else
                {
                    Console.Clear();
                    mensagemVendas();
                    Console.WriteLine("Opção inválida. Nenhuma alteração foi realizada.");
                }

                Thread.Sleep(1500);
                Console.Clear();
                GerenciarVendas(emailLogado);
            }

            public void ExcluirPedido(string emailLogado, int codigoConsulta)
            {
                string conString = @"Data Source=DESKTOP-UUDL5AC\SQLEXPRESS;Initial Catalog=SafeEats;Integrated Security=True;";

                Console.Clear();
                mensagemVendas();

                // Confirmação de exclusão
                Console.Write("Tem certeza de que deseja excluir este pedido? (S/N): ");
                string confirmacao = Console.ReadLine();

                if (confirmacao.ToUpper() == "S")
                {
                    // Exclui o pedido do banco de dados
                    using (SqlConnection conexao = new SqlConnection(conString))
                    {
                        conexao.Open();

                        string excluirPedidoQuery = "DELETE FROM Pedido WHERE codPedido = @codPedido";
                        SqlCommand cmdExcluirPedido = new SqlCommand(excluirPedidoQuery, conexao);
                        // Supondo que você tenha o código do pedido armazenado em uma variável chamada codigoConsulta
                        cmdExcluirPedido.Parameters.AddWithValue("@codPedido", codigoConsulta);

                        int linhasAfetadas = cmdExcluirPedido.ExecuteNonQuery();

                        if (linhasAfetadas > 0)
                        {
                            Console.WriteLine("Pedido excluído com sucesso!");
                        }
                        else
                        {
                            Console.WriteLine("Falha ao excluir o pedido.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Exclusão cancelada.");
                }
            }


            static void Main(string[] args)
            {
                //TelaLogin tl = new TelaLogin();
                //tl.menuInicial();

                ProgramaGeral pg = new ProgramaGeral();
                //pg.menuPrincipalCliente("test");
                pg.menuPrincipalAdmin("test");
            }
        }
    }
}