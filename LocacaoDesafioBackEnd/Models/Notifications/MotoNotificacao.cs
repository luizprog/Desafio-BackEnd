namespace LocacaoDesafioBackEnd.Models.Notifications;
public class MotoNotificacao
{
    public int Id { get; set; }
    public int NotificacaoId { get; set; }
    public int MotoId { get; set; }
    public Notificacao Notificacao { get; set; }
}