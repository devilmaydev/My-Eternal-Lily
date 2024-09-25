using System.Collections;
using _MAIN.Scripts.Core.Characters;
using _MAIN.Scripts.Core.Characters.Types;
using _MAIN.Scripts.Core.Dialogue;
using _MAIN.Scripts.Core.GraphicPanel;
using UnityEngine;

public class PlayTestConversation : MonoBehaviour
{
    void Start() => StartCoroutine(PrimeiroAto());
    IEnumerator PrimeiroAto()
    {
        var Spada = CharacterManager.Instance.CreateCharacter("Conor Spada") as CharacterSprite;
        var Mae = CharacterManager.Instance.CreateCharacter("Mãe") as CharacterSprite;
        var Avo = CharacterManager.Instance.CreateCharacter("Avó") as CharacterSprite;
        var Alex = CharacterManager.Instance.CreateCharacter("Alex") as CharacterSprite;
        var Nano = CharacterManager.Instance.CreateCharacter("Nano") as CharacterSprite;
        var Bona = CharacterManager.Instance.CreateCharacter("Bona") as CharacterText;
        var Cliente = CharacterManager.Instance.CreateCharacter("Cliente") as CharacterText;
        
        var panel = GraphicPanelManager.Instance.GetPanel("Background");
        var bgMainLayer = panel.GetLayer(0, true);
        
        var bTPitchBlack = Resources.Load<Texture>("Graphics/Transition Effects/pitchBlack");
        
        Spada.SetPosition(new Vector2(-1f, 0f));
        Mae.SetPosition(new Vector2(1.2f, 0f));
        Avo.SetPosition(new Vector2(1.2f, 0f));
        Alex.SetPosition(new Vector2(1.2f, 0f));
        Nano.SetPosition(new Vector2(1.2f, 0f));
        
        yield return DialogueSystem.Instance.Say("narrator", "Som de despertador");
        yield return DialogueSystem.Instance.Say("narrator", "Conor acorda e desliga o despertador");

        bgMainLayer.SetTexture("Graphics/PlayTest/quarto_spada", blendingTexture: bTPitchBlack);
        
        Spada.MoveToPosition(new Vector2(0.5f, 0f), 2f, true);
        Spada.SetSprite(Spada.GetSprite("spada_tired"));
        
        yield return Spada.Say("Mas já...? {a}Não era noite tipo, uns 5 minutos atrás?");
        yield return Spada.Say("Não é como se tivesse algo importante para fazer tão cedo.");
        
        yield return Mae.Say("CONNIE! {a}LEVANTA! {a}O REGISTRO TÁ VAZANDO DE NOVO!");
        yield return Mae.Say("Connie! {a}Já levantou!?");
        yield return Mae.Say("CONNIE!!!");
        
        Spada.SetSprite(Spada.GetSprite("spada_surprised"));
        yield return Spada.Say("Tô indo!");
        
        yield return DialogueSystem.Instance.Say("narrator", "Conor levanta depressa e percebe o braço ciborgue fazendo barulhos estranhos");
        
        Spada.SetSprite(Spada.GetSprite("spada_thinking"));
        
        yield return Spada.Say("Talvez um pouquinho de graxa nesse braço ajude.");
        
        Spada.MoveToPosition(new Vector2(-0.5f, 0f));
        
        yield return new WaitForSeconds(1);
        bgMainLayer.SetTexture("Graphics/PlayTest/cozinha", blendingTexture: bTPitchBlack);
        
        yield return DialogueSystem.Instance.Say("narrator", "Sala da casa dos Spada.");
        yield return DialogueSystem.Instance.Say("narrator", "Conor entra na sala.");
        
        Spada.MoveToPosition(new Vector2(0.2f, 0f));
        
        Spada.SetSprite(Spada.GetSprite("spada_nervous"));
        yield return Spada.Say("Foi mal, mãe.");
        
        Mae.MoveToPosition(new Vector2(0.8f, 0f));
        
        yield return Mae.Say("Desculpas não consertam o registro.");
        yield return Mae.Say("Sério, o relógio lá fora tá girando adoidado! {a}Quero nem ver quanto vai vir de conta esse mês.");
        
        Spada.SetSprite(Spada.GetSprite("spada_laugh"));
        
        yield return Spada.Say("A fita só deve ter molhado um pouco. Mas olha só, tá funcionando bem! Aguentou 2 semanas dessa vez!");
        
        Spada.SetSprite(Spada.GetSprite("spada_idle"));
        
        yield return Mae.Say("Claro, você e sua fita dos milagres...");
        yield return Mae.Say("Vem cá, tem coisas mais importantes pra você gastar seu salário do que com Silver Tape.");
        
        //Escolha A
        yield return Spada.Say("Relaxa, esse tipo de coisa nunca é demais.");
        
        yield return Mae.Say("Que pensamento de pobre!");
        
        Spada.SetSprite(Spada.GetSprite("spada_nervous"));
        
        yield return Spada.Say("Mãe, a gente é pobre...");
        yield return Mae.Say("De dinheiro, não da mente.");
        
        yield return DialogueSystem.Instance.Say("narrator", "Silêncio Constrangedor...");
        
        Spada.SetSprite(Spada.GetSprite("spada_idle"));
        
        yield return Spada.Say("A vovó ainda não levantou?");
        yield return Mae.Say("Essa hora? {a}Sua avó parece que está voltando na fase de adolescente, só dorme até meio dia.");
        yield return Mae.Say("Bom, não é como se ela tivesse muita coisa pra fazer de toda forma com aquela idade…");
        
        //Escolha A
        yield return Spada.Say("Quem sabe ela já acordou nessa hora. Vou lá dar uma olhada.");
        
        Spada.MoveToPosition(new Vector2(-0.2f, 0f));
        Mae.MoveToPosition(new Vector2(1.2f, 0f), speed: 4f);
        Mae.Hide();
        
        yield return new WaitForSeconds(1);
        bgMainLayer.SetTexture("Graphics/PlayTest/quarto_vo", blendingTexture: bTPitchBlack);
        
        yield return DialogueSystem.Instance.Say("narrator", "Quarto da Avó");
        
        yield return Spada.MoveToPosition(new Vector2(0.2f, 0f));
        
        yield return DialogueSystem.Instance.Say("narrator", "Som de batidas");
        
        yield return Spada.Say("Vó? {a}Tá acordada?");
        
        yield return Avo.MoveToPosition(new Vector2(0.8f, 0f));
        
        yield return Avo.Say("É claro que eu estou. {a}Sua mãe não fechou a cortina ontem de noite e eu acordei com o sol na minha fuça.");
        yield return Spada.Say("A senhora deveria aproveitar, tão raro ver um sol nessa cidade hoje em dia.");
        yield return Avo.Say("E eu lá quero ver o sol essa hora da manhã, menino? {a}Já acordei cedo demais na minha vida, agora eu vou aproveitar.");
        yield return Spada.Say("Tá certo...");
        yield return Spada.Say("Olha, hoje eu vou fazer um turno extra lá no bar. Então não se preocupe se eu chegar tarde, tá bom?");
        yield return Avo.Say("Meu filho, não fica fazendo tanto esforço por causa de dinheiro...");
        yield return Avo.Say("Você e sua mãe ficam trabalhando dobrado e depois ficam exaustos.");
        
        Spada.SetSprite(Spada.GetSprite("spada_tired"));
        yield return Spada.Say("É o jeito né.");
        
        Spada.SetSprite(Spada.GetSprite("spada_laugh"));
        yield return Spada.Say("Mas fica tranquila, esse turno extra vai pagar bem. {a}Quem sabe dá pra arrumar o registro finalmente e guardar mais dinheiro para consertar suas pernas? ");
        
        Spada.SetSprite(Spada.GetSprite("spada_idle"));
        yield return Avo.Say(".........");
        yield return Avo.Say("Vai logo, se não você perde o ônibus e chega atrasado.");
        yield return Spada.Say("Pode deixar, até depois vó!");
        yield return Avo.Say("Até.");
        
        Spada.MoveToPosition(new Vector2(-0.2f, 0f));
        Avo.MoveToPosition(new Vector2(1.2f, 0f));
        Avo.Hide();
        
        yield return new WaitForSeconds(1);
        bgMainLayer.SetTexture("Graphics/PlayTest/bar_manha", blendingTexture: bTPitchBlack);
        
        yield return DialogueSystem.Instance.Say("narrator", "Bar e Café Barão");
        yield return DialogueSystem.Instance.Say("narrator", "Música de bar chique e sons de pessoas conversando");
        
        Spada.SetSprite(Spada.GetSprite("work_idle"));
        
        yield return Spada.MoveToPosition(new Vector2(0.2f, 0f));
        
        Spada.SetSprite(Spada.GetSprite("work_laugh"));
        
        yield return Spada.Say("Ufa, acho que nunca vi esse lugar tão cheio no meio da semana.");
        
        Spada.SetSprite(Spada.GetSprite("work_idle"));
        
        yield return DialogueSystem.Instance.Say("narrator", "Bona, amiga do Conor, entra em cena");
        
        yield return Bona.Say("Nem me fala. Plena quarta-feira de tarde e esse lugar lotado. {a}Eita povo desocupado.");
        yield return Bona.Say("Cê tava a todo vapor fazendo as bebidas lá trás.");
        
        Spada.SetSprite(Spada.GetSprite("work_laugh"));
        
        yield return Spada.Say("Sinceramente, não quero ver uma garrafa sifão pelo resto da semana…");
        yield return Bona.Say("Também, fazer chantilly com uma garrafa daquelas e com uma mão só é complicado mesmo.");
        
        Spada.SetSprite(Spada.GetSprite("work_tired"));
        yield return Spada.Say("Ah, eu até tentei usar ele na hora que abriu, mas ta fazendo uns barulhos estranhos e travando bastante mesmo com a graxa.");
        
        Spada.SetSprite(Spada.GetSprite("work_idle"));
        yield return Bona.Say("Vish. Tá precisando atualizar o sistema desse braço aí. Tá tão detonado que parece que foi feito no século passado!");
        
        Spada.SetSprite(Spada.GetSprite("work_nervous"));
        yield return Spada.Say("Ei, ele não é detonado! É autêntico.");
        yield return Bona.Say("Detonado, xoxo, capenga e enferrujado.");
        yield return Bona.Say("Olha, eu conheço um cara que consegue fazer um upgrade daora nesse teu braço e ele nunca mais vai dar problema pra você.");
        
        Spada.SetSprite(Spada.GetSprite("work_idle"));
        
        yield return DialogueSystem.Instance.Say("narrator", "Bona envia por mensagem uma imagem da fachada de uma loja");
        
        Spada.SetSprite(Spada.GetSprite("work_thinking"));
        
        yield return Spada.Say("Assistência FNA. Nossa, é lá na República…");
        yield return Bona.Say("Amigo, quanto mais pro centro da cidade, melhor é a qualidade.");
        
        Spada.SetSprite(Spada.GetSprite("work_nervous"));
        
        yield return Spada.Say("..................{a}É exatamente o contrário!");
        yield return Bona.Say("Confia, o preço é bom e eu conheço o cara. Vai sem medo!");
        
        Spada.SetSprite(Spada.GetSprite("work_idle"));
        
        yield return Spada.Say("Tá, talvez dê pra ir lá com o dinheiro da gorjeta que vou receber hoje.");
        yield return Bona.Say("É sério que você vai ficar pro turno da noite? Cara, aqui vira um inferno quando o bar abre!");
        yield return Spada.Say("O registro lá de casa tá vazando ainda mais, nem minhas fitas estão aguentando.");
        yield return Spada.Say("Fora que… {a}Não falta tanto assim para eu conseguir consertar as pernas da minha avó. {a}Então ela não vai precisar mais ficar presa dentro de casa com aquela cadeira de rodas.");
        yield return Bona.Say("Poxa amigo… Já sabe onde vai ficar nesse turno?");
        yield return Spada.Say("Vou ficar no balcão cuidando dos drinks e servindo umas mesas.");
        yield return Bona.Say("Bem, então boa sorte. Até amanhã!");
        
        yield return Spada.MoveToPosition(new Vector2(-0.2f, 0f));
        
        yield return new WaitForSeconds(1);
        bgMainLayer.SetTexture("Graphics/PlayTest/bar_noite", blendingTexture: bTPitchBlack);
        
         yield return DialogueSystem.Instance.Say("narrator", "Anoitece.");
         yield return DialogueSystem.Instance.Say("narrator", "Cyber Jazz e pessoas conversando.");
        
         yield return Spada.MoveToPosition(new Vector2(0.5f, 0f));
        
         Spada.SetSprite(Spada.GetSprite("alt_work_shock"));
        
         yield return Spada.Say("A Bona tinha razão, esse lugar fica muito pior durante a noite, eu respiro e já tem cliente!");
        
         Spada.SetSprite(Spada.GetSprite("alt_work_laugh"));
        
         yield return Spada.Say("Pelo menos o pagamento vai ser bom, daí posso ver para consertar esse braço de uma vez.");
        
         Spada.SetSprite(Spada.GetSprite("alt_work_idle"));
        
         yield return Nano.Say("Com licença-");
         yield return Spada.Say("Ah, mas até que a música é divertida também-");
         yield return Nano.Say("Com licença.");
        
         Spada.SetSprite(Spada.GetSprite("alt_work_surprised"));
        
         yield return Spada.Say("Ah! Desculpe, senhor!");
        
         Spada.MoveToPosition(new Vector2(0.2f, 0f));
         yield return Nano.MoveToPosition(new Vector2(0.8f, 0f));
        
         yield return Nano.Say("Não, relaxa, tá de boa.");
        
        Spada.SetSprite(Spada.GetSprite("alt_work_idle"));
        
        yield return Spada.Say("Sinto muito, o que gostaria?");
        yield return Nano.Say("Quê que cês tem que é forte e barato?");
        
        Spada.SetSprite(Spada.GetSprite("alt_work_laugh"));
        
        yield return Spada.Say("Ah, tem a dose de cachaça se você quer algo rápido, ou então tem a caipirinha de limão que tá no precinho hoje.");
        
        Nano.SetSprite(Nano.GetSprite("nano_thinking"));
        yield return Nano.Say("Quanto.");
        
        Spada.SetSprite(Spada.GetSprite("alt_work_idle"));
        yield return Spada.Say("R$19,90");
        
        Nano.SetSprite(Nano.GetSprite("nano_idle"));
        yield return Nano.Say("Desce essa.");
        
        Spada.SetSprite(Spada.GetSprite("alt_work_laugh"));
        yield return Spada.Say("Pra já, chefia");
        
        yield return Nano.MoveToPosition(new Vector2(1.2f, 0f));
        yield return Spada.MoveToPosition(new Vector2(-0.2f, 0f));
        
        yield return DialogueSystem.Instance.Say("narrator", "Conor prepara o drink.");
        
        yield return Spada.MoveToPosition(new Vector2(0.5f, 0f));
        
        yield return Spada.Say("Primeiro drink complicado da noite feito com sucesso e sem um braço, senhoras e senhores!");
        
        Spada.SetSprite(Spada.GetSprite("alt_work_idle"));
        
        yield return Spada.Say("Espero que aquele cara goste, caprichei na cachaça pra ele.");
        
        Spada.SetSprite(Spada.GetSprite("alt_work_thinking"));
        
        yield return Spada.Say("...........................");
        yield return Spada.Say("Meu braço não deu mais problema desde manhã… Mas se eu continuar sem usar ele, talvez ele fique ainda mais bixado…");
        yield return Spada.Say("............");
        
        yield return DialogueSystem.Instance.Say("narrator", "Som de metal batendo de leve contra vidro quando Conor segura o copo com o braço mecânico");
        
        Spada.SetSprite(Spada.GetSprite("alt_work_idle"));
        
        yield return Spada.Say("Beleza, é só ir devagar que vai dar certo. Sem movimentos bruscos.");
        yield return Spada.Say("Devagar… Devagar…");
        
        yield return Spada.MoveToPosition(new Vector2(0.2f, 0f), 4f, true);
        
        yield return Spada.Say("Prontinho! Aqui sua bebida, moço-");
        
        yield return DialogueSystem.Instance.Say("narrator", "Som de metal batendo de leve contra vidro quando Conor segura o copo com o braço mecânico");
        
        yield return Nano.MoveToPosition(new Vector2(0.8f, 0f));
        
        yield return Nano.Say("Cê tá bem?");
        yield return Spada.Say("O quê-");
        
        yield return DialogueSystem.Instance.Say("narrator", "O copo explode na mão de Conor e o líquido cai no colo de Nano");
        
        Nano.SetSprite(Nano.GetSprite("nano_upset"));
        Spada.SetSprite(Spada.GetSprite("alt_work_shock"));
        yield return Spada.Say("PORRA!");
        
        Spada.SetSprite(Spada.GetSprite("alt_work_surprised"));
        yield return Spada.Say("D-Desculpa moço! Meu braço tava dando problema hoje de manhã e eu queria saber se ele ainda tava ruim ou—-");
        
        Nano.SetSprite(Nano.GetSprite("nano_kindamad"));
        yield return Nano.Say("*suspiro*.... Tá tranquilo.");
        yield return Spada.Say("Hã? Mas-");
        
        Nano.SetSprite(Nano.GetSprite("nano_upset"));
        yield return Nano.Say("Eu disse que tá tudo bem, entendeu?");
        
        yield return DialogueSystem.Instance.Say("narrator", "Nano deixa algumas moedas no balcão");
        
        yield return Nano.MoveToPosition(new Vector2(1.2f, 0f));
        yield return Spada.MoveToPosition(new Vector2(0.5f, 0f), 2f, true);
        
        Spada.SetSprite(Spada.GetSprite("alt_work_thinking"));
        yield return Spada.Say("........{a}Ele só saiu e deixou o dinheiro aqui?");
        
        Spada.SetSprite(Spada.GetSprite("alt_work_laugh"));
        yield return Spada.Say("Bom, dinheiro é dinheiro.");
        
        yield return DialogueSystem.Instance.Say("narrator", "Conor pega as moedas");
        
        Spada.SetSprite(Spada.GetSprite("alt_work_idle"));
        
        yield return Cliente.Say("Ei! Pode limpar nossa mesa? Meu amigo acabou derrubando a bebida aqui.");
        
        Spada.SetSprite(Spada.GetSprite("alt_work_laugh"));
        
        yield return Spada.Say("Ah sim, claro!");
        
        yield return DialogueSystem.Instance.Say("narrator", "Conor sai correndo de trás do balcão");
        yield return DialogueSystem.Instance.Say("narrator", "Algumas faíscas começam a sair do braço mecânico e fazer barulhos de máquina rangendo");
        
        Spada.SetSprite(Spada.GetSprite("alt_work_shock"));
        
        yield return Spada.Say("O quê!? De novo não-");
        yield return Cliente.Say("Licença, dá pra você fazer isso log- HÃ!?");
        
        Spada.SetSprite(Spada.GetSprite("alt_work_surprised"));
        
        yield return DialogueSystem.Instance.Say("narrator", "O braço começa a se mexer sozinho e acaba acertando o cliente");
        
        yield return Cliente.Say("AAAAAAAAAAAAH MEU OLHO!!!");
        yield return Spada.Say("MEU DEUS, VOCÊ TA BEM!?");
        yield return Cliente.Say("É CLARO QUE NÃO! TU ME DEU UM SOCO, CARA! CÊ TÁ MALUCO!?");
        
        yield return DialogueSystem.Instance.Say("narrator", "Pessoas cochichando e tentando ajudar o Cliente");
        
        yield return Spada.Say("Ai, minha nossa, eu sinto muito! Eu não sei o que aconteceu com o meu braço.");
        yield return Cliente.Say("CADÊ O GERENTE!? EU VOU PROCESSAR ESSE LUGAR! MEU PAI É ADVOGADO! {a}NÃO, EU VOU PROCESSAR VOCÊ, SEU FILHO DA PUTA! VOCÊ VAI PAGAR POR ISSO!");
        yield return Spada.Say("*suspiro* Eu tô tão ferrado…");
        
        yield return Spada.MoveToPosition(new Vector2(-0.2f, 0f));
        
        yield return new WaitForSeconds(1);
        bgMainLayer.SetTexture("Graphics/PlayTest/cozinha", blendingTexture: bTPitchBlack);
        
        yield return DialogueSystem.Instance.Say("narrator", "Casa dos Spada");
        yield return DialogueSystem.Instance.Say("narrator", "A porta abre e fecha com um baque");
        
        Mae.SetPosition(new Vector2(1.2f, 0f));
        Mae.Show();
        Mae.MoveToPosition(new Vector2(0.8f, 0f));
        
        
        yield return Mae.Say("Connie! {a}Chegou tão cedo, teve algum problema no trabalho?");
        
        Spada.SetSprite(Spada.GetSprite("spada_tired"));
        
        yield return Spada.MoveToPosition(new Vector2(0.2f, 0f), 4f, true);
        
        yield return Spada.Say("Não, mãe. Foi tudo bem! Eu só fui dispensado mais cedo porque…");
        yield return Spada.Say("Tinha outro cara escalado pra fazer meio turno e calhou de ser o mesmo que o meu.");
        yield return Mae.Say("É sério isso? Não avisaram nada pra você? Sinceramente, eu espero que eles tenham te pagado certinho e não só por meio período.");
        yield return Spada.Say("Não não, eles pagaram direitinho pra mim, não se preocupa…");
        yield return Mae.Say("O que foi, Connie? Porque essa cara tão xoxa, filho…");
        yield return Spada.Say("É que meu braço deu problema, acho que vou ter que ir no concerto no fim das contas…");
        yield return Mae.Say("Não fica triste por isso. Lembra, a oficina do Seu Marcelo sempre consegue remendar ele pra você.");
        
        Spada.SetSprite(Spada.GetSprite("spada_thinking"));
        
        yield return Spada.Say("Mãe, aquele cara me cobra uma fortuna só pra apertar meus parafusos…");
        yield return Mae.Say("Mas ele te conhece desde criança. Não se pode confiar em qualquer um pra concertar esse tipo de coisa!");
        
        Spada.SetSprite(Spada.GetSprite("spada_tired"));
        
        yield return Spada.Say("*suspiro* Tá legal, amanhã eu vou lá e vejo se ele tem um braço reserva enquanto esse aqui arruma.");
        yield return Mae.Say("Isso aí. Agora vai descansar que já tá muito tarde.");

        Spada.SetSprite(Spada.GetSprite("spada_idle"));

        yield return Mae.Say("Ah! Sua avó mandou um beijo pra você e disse que tem muito orgulho do netinho dela.");
        yield return Spada.Say("Dá outro nela depois.");
        yield return Spada.Say("Vou ir deitar, boa noite mãe.");
        yield return Mae.Say("Boa noite, Connie.");
        
        Spada.Hide();
        Mae.Hide();
        Mae.MoveToPosition(new Vector2(1.2f, 0f));
        Spada.MoveToPosition(new Vector2(-0.2f, 0f));
        
        yield return new WaitForSeconds(1);
        bgMainLayer.SetTexture("Graphics/PlayTest/quarto_spada", blendingTexture: bTPitchBlack);
        
        yield return DialogueSystem.Instance.Say("narrator", "Quarto do Conor");
        yield return DialogueSystem.Instance.Say("narrator", "Som de porta abrindo e fechando");
        
        Spada.MoveToPosition(new Vector2(0.5f, 0f));
        Spada.Show();
        
        Spada.SetSprite(Spada.GetSprite("spada_tired"));
        yield return Spada.Say("*Suspiro* É vovó, seu netinho que você tem tanto orgulho tá desempregado agora, yeeey…");
        
        Spada.SetSprite(Spada.GetSprite("spada_pain"));

        yield return Spada.Say("Pelo menos deu pra pegar uma boa gorjeta do turno da manhã, dá pra pagar o registro tranquilo.");
        yield return Spada.Say("Agora com a gorjeta da noite… Ah, não dá nem pra dar entrada num concerto lá no Seu Marcelo!");
        yield return Spada.Say("É isso, tô ferrado…");
        
        Spada.MoveToPosition(new Vector2(-0.2f, 0f));
        Spada.Hide();
        
        yield return DialogueSystem.Instance.Say("narrator", "Conor fecha os olhos");
        yield return DialogueSystem.Instance.Say("narrator", "Barulho de notificação de mensagem");
        yield return DialogueSystem.Instance.Say("narrator", "Conor abre os olhos");
        yield return DialogueSystem.Instance.Say("narrator", "[NOVA MENSAGEM DE BONA:\nAmg! Esqueci de te mandar o link do Enstragrama daquela loja q te falei\nDá uma olhada lá depois bjsssss <3]");
        yield return Spada.Say("Não resta outra opção, né?");
        
        yield return new WaitForSeconds(1);
        bgMainLayer.SetTexture("Graphics/PlayTest/rua_republica", blendingTexture: bTPitchBlack);
        
        yield return DialogueSystem.Instance.Say("narrator", "No dia seguinte...");
        
        Spada.SetSprite(Spada.GetSprite("spada_thinking"));
        Spada.Show();
        Spada.MoveToPosition(new Vector2(0.5f, 0f));
        
        yield return Spada.Say("Ok, é mais longe do que eu pensei…");
        yield return Spada.Say("Vejamos, já vi alguém ser assaltado, um cara vestido de porco cantando em uma praça, um provável cadáver em uma esquina…");
        
        Spada.SetSprite(Spada.GetSprite("spada_nervous"));
        
        yield return Spada.Say("É, não tavam mentindo quando disseram que o Centro de São Paulo é um dos lugares dessa cidade…");
        
        Spada.Hide();
        
        yield return new WaitForSeconds(1);
        bgMainLayer.SetTexture("Graphics/PlayTest/loja_frente", blendingTexture: bTPitchBlack);
        
        yield return DialogueSystem.Instance.Say("narrator", "Spada se encontra de frente para a loja.");
        
        Spada.SetSprite(Spada.GetSprite("spada_idle"));
        
        yield return Spada.Say("Parece antiga...");
        
        yield return new WaitForSeconds(1);
        bgMainLayer.SetTexture("Graphics/PlayTest/loja_interior", blendingTexture: bTPitchBlack);
        
        yield return DialogueSystem.Instance.Say("narrator", "Dentro da loja");
        
        Spada.Show();
        Spada.MoveToPosition(new Vector2(0.2f, 0f));
        
        yield return Spada.Say("Oi? Licença?");
        
        yield return DialogueSystem.Instance.Say("narrator", "Silêncio...");
        
        Spada.SetSprite(Spada.GetSprite("spada_thinking"));
        
        yield return Spada.Say("Ué, tá fechado? Mas na página deles tava escrito que-");
        
        yield return Alex.MoveToPosition(new Vector2(0.8f, 0f));
        
        Spada.SetSprite(Spada.GetSprite("spada_surprised"));
        Alex.SetSprite(Alex.GetSprite("alex_laugh"));
        
        yield return Alex.Say("Pelos códigos, um cliente!");
        yield return Spada.Say("AH! Quase tu me mata de susto, moça!");
        
        Alex.SetSprite(Alex.GetSprite("alex_smile"));
        Spada.SetSprite(Spada.GetSprite("spada_idle"));
        
        yield return Alex.Say("Foi mal! Sempre me empolgo quando vou falar com uma pessoa diferente.");
        yield return Alex.Say("Eu sou Alex. E você é?");
        
        Alex.SetSprite(Alex.GetSprite("alex_idle"));
        Spada.SetSprite(Spada.GetSprite("spada_laugh"));
        
        yield return Spada.Say("Conor Spada, prazer em te conhecer.");
        
        Spada.SetSprite(Spada.GetSprite("spada_nervous"));
        yield return Spada.Say("Ah… Desculpa se eu tô sendo indelicado e tals, mas você não me parece tão…");
        
        Spada.SetSprite(Spada.GetSprite("spada_idle"));
        Alex.SetSprite(Alex.GetSprite("alex_smile"));
        yield return Alex.Say("Humana? {a}Isso porquê eu de fato não sou.");
        yield return Alex.Say("Sou um sistema programado com emoções, pensamentos e uma consciência própria, com capacidade de gerar superfícies sólidas através de átomos de luz e converter dióxido de carbono em energia funcional e limpa para todo tipo de dispositivo.");
        
        Alex.SetSprite(Alex.GetSprite("alex_idle"));
        Spada.SetSprite(Spada.GetSprite("spada_nervous"));
        yield return Spada.Say("Eh?");
        
        Alex.SetSprite(Alex.GetSprite("alex_smile"));
        yield return Alex.Say("Uma IA com corpo físico. Com um plus em habilidades.");
        
        Alex.SetSprite(Alex.GetSprite("alex_idle"));
        Spada.SetSprite(Spada.GetSprite("spada_laugh"));
        yield return Spada.Say("Ah tá. Caramba! Você tem um corpo mesmo! Eu achei que IAs não conseguiam fazer isso…");
        
        Alex.SetSprite(Alex.GetSprite("alex_smile"));
        Spada.SetSprite(Spada.GetSprite("spada_idle"));
        yield return Alex.Say("Se fosse há 100 anos atrás, talvez eu conseguisse só criar respostas rápidas e umas imagens bem meia boca. Tá precisando se atualizar, rapaz.");
        
        Alex.SetSprite(Alex.GetSprite("alex_idle"));
        Spada.SetSprite(Spada.GetSprite("spada_nervous"));
        yield return Spada.Say("Ha ha… Não é muito o meu forte…");
        
        Alex.SetSprite(Alex.GetSprite("alex_smile"));
        Spada.SetSprite(Spada.GetSprite("spada_idle"));
        yield return Alex.Say("Enfim, vou chamar alguém pra vir te atender. Me dá um segundinho.");
        yield return Alex.Hide();
        
        //Escolha A
        yield return DialogueSystem.Instance.Say("narrator", "Conor espera.");
        yield return DialogueSystem.Instance.Say("narrator", "Som de mensagem");
        yield return DialogueSystem.Instance.Say("narrator", "[NOVA MENSAGEM DE BONA:\nE aí???????]");
        
        yield return Spada.Say("Eu tô aqui na loja já. Veio uma mulher super maneira falar comigo e na realidade era uma IA super avançada!");
        
        yield return DialogueSystem.Instance.Say("narrator", "[Eu falei pra tu confiar em mim >:)]");
        yield return DialogueSystem.Instance.Say("narrator", "[Aliás, eu e o pessoal achamos uma casa de City Pop que a gente tá pensando em ir hoje de noite. Tá afim??]");
        
        yield return Spada.Say("Vou não, minha grana tá muito curta e eu tô vendo se tem emprego aqui pelo centro. Mas aproveitem por aí e dá um abraço no pessoal por mim!");
        
        yield return DialogueSystem.Instance.Say("narrator", "[Tá bom!! <3]");
        
        yield return DialogueSystem.Instance.Say("narrator", "Uma voz surge.");
        
        yield return Nano.Say("Espero que cê não esteja fazendo esse fuzuê todo por causa de outro mendigo que queria usar o banheiro…");
        yield return Alex.Say("Dessa vez não é! Eu juro!");
        
        Spada.SetSprite(Spada.GetSprite("spada_thinking"));
        yield return Spada.Say("Eu conheço essa voz…");
        
        Nano.SetSprite(Nano.GetSprite("nano_idle"));
        Nano.SetPosition(new Vector2(1.2f,0f));
        Nano.Show();
        Nano.MoveToPosition(new Vector2(0.7f, 0f));
        
        Alex.SetSprite(Alex.GetSprite("alex_idle"));
        Alex.SetPosition(new Vector2(1.2f,0f));
        Alex.Show();
        Alex.MoveToPosition(new Vector2(0.9f, 0f));
        
        Spada.SetSprite(Spada.GetSprite("spada_surprised"));
        yield return Spada.Say("Ah… É o cara do bar… Ferrou.");
        
        Spada.SetSprite(Spada.GetSprite("spada_nervous"));
        yield return Spada.Say("Err oi…");
        
        Nano.SetSprite(Nano.GetSprite("nano_kindamad"));
        yield return Nano.Say("Oi.");
        
        Alex.SetSprite(Alex.GetSprite("alex_baffled"));
        yield return Alex.Say("…Eu hein, que tensão estranha entre vocês dois…");
        
        Alex.SetSprite(Alex.GetSprite("alex_idle"));
        Nano.SetSprite(Nano.GetSprite("nano_thinking"));
        yield return Nano.Say("*suspiro* Que que cê tá fazendo aqui?");
        
        yield return Spada.Say("U-Uma amiga me recomendou vir aqui pra arrumar o meu braço… Ela disse que você podia me ajudar.");
        yield return Nano.Say("...........");
        
        Nano.SetSprite(Nano.GetSprite("nano_idle"));
        yield return Nano.Say("Tá, me mostra.");
        
        Spada.SetSprite(Spada.GetSprite("spada_thinking"));
        yield return Spada.Say("'Ele não parece que tá bravo mais…'");
        Spada.SetSprite(Spada.GetSprite("spada_idle"));
        
        yield return DialogueSystem.Instance.Say("narrator", "Conor remove o braço e coloca no balcão");
        yield return DialogueSystem.Instance.Say("narrator", "Silêncio enquanto Nano analisa o braço");
        
        yield return Nano.Say("Quê que você acha?");
        
        Alex.SetSprite(Alex.GetSprite("alex_baffled"));
        yield return Alex.Say("É o pior modelo que eu já vi.");
        
        Alex.SetSprite(Alex.GetSprite("alex_idle"));
        Nano.SetSprite(Nano.GetSprite("nano_thinking"));
        yield return Nano.Say("Com certeza. Remendo com fita, parafusos soltos, soldagem mal feita, ferrugem…");
        
        Alex.SetSprite(Alex.GetSprite("alex_smile"));
        Nano.SetSprite(Nano.GetSprite("nano_idle"));
        yield return Alex.Say("Rapaz, você tem é sorte de não ter pego tétano ou levado um choque violento com isso.");
        
        Alex.SetSprite(Alex.GetSprite("alex_idle"));
        Spada.SetSprite(Spada.GetSprite("spada_nervous"));
        yield return Spada.Say("Tá legal, eu já entendi.");
        
        Spada.SetSprite(Spada.GetSprite("spada_thinking"));
        yield return Spada.Say("Tem como arrumar ele?");
        
        Nano.SetSprite(Nano.GetSprite("nano_thinking"));
        yield return Nano.Say("Arrumar? Isso aqui? Não precisa ser nenhum influente em peças pra saber que isso devia estar no lixo.");
        
        Alex.SetSprite(Alex.GetSprite("alex_smile"));        
        Nano.SetSprite(Nano.GetSprite("nano_idle"));
        yield return Alex.Say("Exato. Isso infringe pelo menos umas 8 normas de saúde pública.");
        
        Alex.SetSprite(Alex.GetSprite("alex_idle"));
        Spada.SetSprite(Spada.GetSprite("spada_nervous"));
        
        yield return Spada.Say("................");
        yield return Spada.Say("O que eu faço então!?");
        
        Nano.SetSprite(Nano.GetSprite("nano_thinking"));
        yield return Nano.Say("Dá pra fazer um do zero. Iria demorar cerca de…");
        
        Alex.SetSprite(Alex.GetSprite("alex_smile"));        
        Nano.SetSprite(Nano.GetSprite("nano_idle"));
        yield return Alex.Say("3 meses.");
        
        Alex.SetSprite(Alex.GetSprite("alex_idle"));
        Spada.SetSprite(Spada.GetSprite("spada_shock"));

        yield return Spada.Say("3 MESES!? E quanto custa isso??");
        yield return Nano.Say("5 mil.");
        
        Spada.SetSprite(Spada.GetSprite("spada_surprised"));
        yield return Spada.Say("CINCO MIL!?!?");
        
        Spada.SetSprite(Spada.GetSprite("spada_nervous"));
        yield return Spada.Say("Moço, cê me desculpa mas eu não tenho esse dinheiro não! Eu vou ficar com meu braço antigo mesmo!");
        yield return Nano.Say("Cê que sabe.");
        
        Spada.Hide();
        Spada.MoveToPosition(new Vector2(-0.2f, 0f));
        Spada.SetSprite(Spada.GetSprite("spada_idle"));
        
        yield return Spada.Say("*suspiro*");
        
        Nano.MoveToPosition(new Vector2(0.2f, 0f));
        Alex.MoveToPosition(new Vector2(0.8f, 0f));
        
        Alex.SetSprite(Alex.GetSprite("alex_baffled"));
        Nano.SetSprite(Nano.GetSprite("nano_idle"));
        yield return Alex.Say("Eu não acredito que você vai perder um cliente só por causa disso!");
        
        Alex.SetSprite(Alex.GetSprite("alex_baffled"));
        Nano.SetSprite(Nano.GetSprite("nano_thinking"));
        yield return Nano.Say("Quê que é? Cê sabe que o máximo que eu sei fazer é consertar celular e computador. Eu tô aliviado que aquele mano não aceitou.");
        
        Alex.SetSprite(Alex.GetSprite("alex_smile"));
        yield return Alex.Say("E quem falou que você não tem o conhecimento disso na palma da sua mão!? Eu posso pesquisar pra você como fazer isso! Ou então tem os cadernos do Fra-");
        
        Alex.SetSprite(Alex.GetSprite("alex_idle"));
        Nano.SetSprite(Nano.GetSprite("nano_upset"));
        yield return Nano.Say("Eu não vou mexer nas coisas dele.");
        
        Alex.SetSprite(Alex.GetSprite("alex_baffled"));
        Nano.SetSprite(Nano.GetSprite("nano_kindamad"));
        yield return Alex.Say("Nano, se a gente conseguir replicar naquela tecnologia e conseguir fazer esse braço funcionar, a gente vai finalmente conseguir fazer essa loja sair das dívidas!");
        
        Alex.SetSprite(Alex.GetSprite("alex_smile"));
        Nano.SetSprite(Nano.GetSprite("nano_idle"));
        yield return Alex.Say("Vai, você sabe que consegue fazer isso!");
        
        Alex.SetSprite(Alex.GetSprite("alex_idle"));
        yield return Nano.Say("……………………………………");
        
        Nano.SetSprite(Nano.GetSprite("nano_thinking"));
        yield return Nano.Say("*suspiro* Saiba que se isso der errado, eu não vou me importar de te reprogramar inteira.");
        
        Alex.SetSprite(Alex.GetSprite("alex_laugh"));
        yield return Alex.Say("Você nunca faria isso e nós dois sabemos disso.");
        
        Nano.Hide();
        Alex.Hide();
        Nano.SetPosition(new Vector2(1.2f, 0f));
        Alex.SetPosition(new Vector2(1.2f, 0f));
        Alex.SetSprite(Alex.GetSprite("alex_idle"));
        Nano.SetSprite(Nano.GetSprite("nano_idle"));
        
        yield return new WaitForSeconds(1);
        bgMainLayer.SetTexture("Graphics/PlayTest/loja_frente", blendingTexture: bTPitchBlack);
        
        yield return DialogueSystem.Instance.Say("narrator", "Do lado de fora da loja.");
        
        Spada.SetSprite(Spada.GetSprite("spada_shock"));
        Spada.Show();
        Spada.MoveToPosition(new Vector2(0.5f, 0f));
        
        yield return Spada.Say("5 mil reais! Isso dava 5 meses do meu antigo salário! Tá compensando mais ir no Seu Marcelo mesmo!");
        
        Spada.SetSprite(Spada.GetSprite("spada_thinking"));
        yield return Spada.Say("Vai sair uma merda? É claro que vai! Mas tá melhor do que gastar tudo e ficar com nome sujo no SIRESA!");
        yield return Alex.Say("Moço! Perai!!!");
        
        Alex.Show();
        Spada.SetSprite(Spada.GetSprite("spada_idle"));
        Spada.MoveToPosition(new Vector2(0.2f, 0f));
        Alex.MoveToPosition(new Vector2(0.8f, 0f));
        
        Spada.SetSprite(Spada.GetSprite("spada_nervous"));
        yield return Spada.Say("Moça, desculpa fazer cê ir até aqui mas eu não posso pagar aquele preço-");
        
        Alex.SetSprite(Alex.GetSprite("alex_smile"));
        yield return Alex.Say("Eu tenho uma proposta para te fazer.");
        
        Alex.SetSprite(Alex.GetSprite("alex_idle"));
        Spada.SetSprite(Spada.GetSprite("spada_thinking"));
        yield return Spada.Say("Hã?");
        
        Alex.SetSprite(Alex.GetSprite("alex_smile"));
        yield return Alex.Say("Pelo o que eu ouvi, você está desempregado e procurando um emprego aqui no centro, certo? {a}O que acha disso: Se trabalhar na loja, você pode pagar o seu braço pelas suas horas de trabalho e ainda ganhar um salário no fim do mês.");
        
        Spada.SetSprite(Spada.GetSprite("spada_nervous"));       
        Alex.SetSprite(Alex.GetSprite("alex_idle"));
        yield return Spada.Say("Você ouviu que eu estava desempregado? Ah… Bem, não é uma proposta ruim… Quanto tempo eu teria que trabalhar?");
        
        Alex.SetSprite(Alex.GetSprite("alex_smile"));
        yield return Alex.Say("De acordo com minhas estatísticas, seria uma média de 40 horas semanais, sendo de segunda à sexta, com exceção de feriados, do período das 10h até às 18h. Por exatos 3 meses.");
        yield return Alex.Say("O que me diz?");
        
        Alex.SetSprite(Alex.GetSprite("alex_idle"));
        Spada.SetSprite(Spada.GetSprite("spada_idle"));       
        yield return Spada.Say("Uau, essa escala tá bem melhor do que no meu antigo emprego…");
        Spada.SetSprite(Spada.GetSprite("spada_laugh"));       
        yield return Spada.Say("Bom, eu aceito!");
        
        Spada.Hide();
        Alex.Hide();
        Spada.SetPosition(new Vector2(-0.2f, 0f));
        Alex.SetPosition(new Vector2(-0.2f, 0f));

        yield return new WaitForSeconds(1);
        bgMainLayer.SetTexture("Graphics/PlayTest/loja_interior", blendingTexture: bTPitchBlack);
        
        yield return DialogueSystem.Instance.Say("narrator", "Dentro da loja.");
        
        Alex.SetSprite(Alex.GetSprite("alex_laugh"));
        Spada.SetSprite(Spada.GetSprite("spada_idle"));      
        
        Spada.Show();
        Alex.Show();
        Nano.Show();
        
        Spada.MoveToPosition(new Vector2(0.1f, 0f));
        Alex.MoveToPosition(new Vector2(0.3f, 0f));
        Nano.MoveToPosition(new Vector2(0.8f, 0f));

        yield return Alex.Say("Diga “oi” para o seu novo balconista!");
        
        Alex.SetSprite(Alex.GetSprite("alex_idle"));
        Nano.SetSprite(Nano.GetSprite("nano_thinking"));
        yield return Nano.Say("Meu novo o que?");
        
        yield return Spada.Say("Ela me disse que eu posso trabalhar aqui para pagar meu braço, e que eu receberia no fim do mês também.");
        
        Nano.SetSprite(Nano.GetSprite("nano_kindamad"));
        yield return Nano.Say("ELA O QUE!?");
        
        Alex.SetSprite(Alex.GetSprite("alex_laugh"));
        yield return Alex.Say("Relaxa, seu salário dá pra pagar ele tranquilamente, e um balconista vai te dar mais liberdade para trabalhar nos fundos com o pedido dos clientes! Eu já pensei em tudo, querido.");
        
        Alex.SetSprite(Alex.GetSprite("alex_idle"));
        Nano.SetSprite(Nano.GetSprite("nano_thinking"));
        yield return Nano.Say("'Não surta, não surta……'");
        
        Spada.SetSprite(Spada.GetSprite("spada_laugh"));
        yield return Spada.Say("Já que agora você é meu novo patrão, nada mais justo do que eu me apresentar. Eu sou Conor Spada!");
        
        Spada.SetSprite(Spada.GetSprite("spada_idle"));
        yield return Nano.Say("Nano e Alex.");
        yield return Nano.Say("Agora, você sabe consertar coisas simples, tipo, um celular?");
        
        Spada.SetSprite(Spada.GetSprite("spada_nervous"));
        yield return Spada.Say("Eu nunca mexi com esse tipo de coisa na realidade, mas eu imagino que não seja tão difícil. E se ficar, {a}dá pra remendar com um pouquinho de fita!");
        
        Nano.SetSprite(Nano.GetSprite("nano_kindamad"));
        yield return Nano.Say("……………… O quê-");

        Spada.Hide();
        Alex.Hide();
        Nano.Hide();
        
        yield return DialogueSystem.Instance.Say("narrator", "Esse foi o fim do primeiro ato. {a}Esperamos que tenha gostado até agora!");
        Alex.SetSprite(Alex.GetSprite("alex_laugh"));
        Alex.SetPosition(new Vector2(0.5f,0f));
        yield return Alex.Show();
        yield return Alex.Say("Por favor avalie o nosso protótipo ao apontar a camera do seu celular para o QR Code ao lado do computador!");
        yield return Alex.Say("Muito Obrigado e até logo!");
    }
}
