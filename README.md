# DigestingDuck

Aplicativo desktop para acompanhar contas, ativos, traders e sinais da IQ Option.
A interface multiplataforma usa **Avalonia 12**, **.NET 9** e
**IqOptionApiDotNet 3.0.4**.

## Windows, macOS e Linux

O workflow **Desktop multiplataforma** gera instaladores independentes do runtime:

| Sistema | Arquiteturas | Formato |
|---|---|---|
| Windows | x64 / ARM64 | Instalador `.exe` por usuário |
| macOS | Intel / Apple Silicon | `.dmg` com aplicativo `.app` |
| Linux Debian/Ubuntu | x64 / ARM64 | `.deb` e pacote portável `.tar.gz` |

Outras distribuições Linux podem usar o pacote portável com as dependências
nativas do Avalonia e .NET. Compatibilidade de compilação não substitui teste em
cada sistema. Os instaladores ainda não têm assinatura comercial/notarização.

## Executar pelo código

```sh
dotnet run --project src/DigestingDuck.Desktop/DigestingDuck.Desktop.csproj
```

A nova aplicação não depende de WPF, Realm, caminhos de unidade Windows nem de
um servidor web. O projeto WPF original continua em `DigestingDuck/` como legado,
mas não faz parte do novo build. Seu workflow foi deixado como execução manual.

## O que está implementado

- Conexão e encerramento de sessão na corretora.
- Consulta de saldos, gráfico de velas, ranking de traders e alertas de preço.
- Monitoramento de operações digitais e cópia de traders selecionados.
- Importação de sinais CSV com seletor de arquivo nativo e fuso horário explícito.
- Limites por sessão e controle de exposição das ordens ainda abertas.
- Histórico local, exportação CSV e conciliação de resultados não recebidos.
- Configuração e histórico em JSON, sem salvar a senha.

A aplicação abre em modo de monitoramento. Para enviar ordens, selecione a conta,
habilite a opção e confirme a execução. Testes usam uma corretora simulada e não
enviam operações. O funcionamento autenticado depende da conta e do protocolo
atual da plataforma; não foi validado com credenciais reais durante a migração.

## Importar sinais

```csv
data;ativo;direção;minutos
2026-10-20T15:00:00-03:00;EURUSD_op;CALL;1
2026-10-20T15:05:00-03:00;EURUSD_OTC;PUT;5
```

Aceita duração de 1, 5 ou 15 minutos, nomes do enum ou IDs numéricos de ativos.
Sinais atrasados além do limite configurado não são enviados. Pare a sessão antes
de importar uma lista. O resultado de uma ordem deve ser confirmado antes de
iniciar uma nova sessão quando há posições pendentes.

## Dados e migração

A nova aplicação guarda `settings.json` e `journal.json` em
`LocalApplicationData/DigestingDuck/desktop`, conforme o sistema operacional.
O banco Realm antigo é preservado, mas **não é importado automaticamente**.
Não há equivalência completa das antigas opções de Soros/Martingale, filtros de
payout e horários; o gerenciamento novo usa entrada fixa e stops de sessão.
As páginas antigas de Chat, Portfólio e Alertas eram parcialmente vazias.

## Testes e distribuição

```sh
dotnet test tests/DigestingDuck.Core.Tests/DigestingDuck.Core.Tests.csproj
```

O Actions testa o núcleo, renderiza as oito telas sem display e publica os pacotes em Windows, Ubuntu e macOS, para
seis runtime identifiers. Artefatos ficam disponíveis em cada execução. Tags
`v*` publicam os instaladores na GitHub Release correspondente; use tags de
prévia enquanto a integração real e a migração de dados não estiverem validadas.

Licença [MIT](LICENSE). Cliente não oficial, sem vínculo com a IQ Option.

### Importar configurações do aplicativo antigo

Feche o aplicativo antigo. Com .NET 9 instalado, execute no checkout:

```sh
dotnet run --project tools/DigestingDuck.LegacyExport -- caminho/DigestingDuck.realm configuracao-legada.json
```

O exportador abre uma cópia temporária do banco; não altera o original e não exporta senhas ou serial. Se houver vários usuários, acrescente o ID desejado como terceiro argumento. Na aba **Gerenciamento**, escolha **Importar configuração antiga (JSON)** com a sessão parada. Email, traders alvo e ativos são transferidos; a configuração anterior e as regras antigas ficam em uma pasta `migration-*` no diretório de dados. Valores e limites atuais são mantidos para revisão. Soros, Martingale e outras regras antigas não são ativados automaticamente. Histórico, saldos e sinais continuam no banco original. A compatibilidade de um banco real ainda precisa ser validada; o exportador depende dos sistemas suportados pelo Realm 20, sem adicionar essa dependência ao aplicativo desktop.
