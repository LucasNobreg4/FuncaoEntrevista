
$(document).ready(function () {
    $('#CPF').mask('000.000.000-00');
    $('#CEP').mask('00000-000');
    $('#Telefone').mask('(00) 0000-00009');

    $('#formCadastro').submit(function (e) {
        e.preventDefault();

        var cliente = {
            "NOME": $(this).find("#Nome").val(),
            "CEP": $(this).find("#CEP").val(),
            "Email": $(this).find("#Email").val(),
            "Sobrenome": $(this).find("#Sobrenome").val(),
            "Nacionalidade": $(this).find("#Nacionalidade").val(),
            "Estado": $(this).find("#Estado").val(),
            "Cidade": $(this).find("#Cidade").val(),
            "Logradouro": $(this).find("#Logradouro").val(),
            "Telefone": $(this).find("#Telefone").val(),
            "CPF": $(this).find("#CPF").val()
        };

        var temBeneficiarios = typeof beneficiarios !== 'undefined' && beneficiarios.length > 0;
        var url = temBeneficiarios ? urlPostComBeneficiarios : urlPost;
        var dados;

        if (temBeneficiarios) {
            dados = {
                model: cliente,
                Beneficiarios: beneficiarios
            };
        } else {
            dados = cliente;
        }

        $.ajax({
            url: url,
            method: "POST",
            contentType: "application/json",
            data: JSON.stringify(dados),
            error:
                function (r) {
                    if (r.status == 400)
                        ModalDialog("Ocorreu um erro", r.responseJSON);
                    else if (r.status == 500)
                        ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
                    else
                        ModalDialog("Ocorreu um erro", "Erro desconhecido: " + r.statusText);
                },
            success:
                function (r) {
                    ModalDialog("Sucesso!", r.Mensagem || r);
                    beneficiarios = [];
                    atualizarTabelaBeneficiarios();
                    $("#formCadastro")[0].reset();
                }
        });
    })

})

function ModalDialog(titulo, texto) {
    var random = Math.random().toString().replace('.', '');
    var texto = '<div id="' + random + '" class="modal fade">                                                               ' +
        '        <div class="modal-dialog">                                                                                 ' +
        '            <div class="modal-content">                                                                            ' +
        '                <div class="modal-header">                                                                         ' +
        '                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>         ' +
        '                    <h4 class="modal-title">' + titulo + '</h4>                                                    ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-body">                                                                           ' +
        '                    <p>' + texto + '</p>                                                                           ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-footer">                                                                         ' +
        '                    <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>             ' +
        '                                                                                                                   ' +
        '                </div>                                                                                             ' +
        '            </div><!-- /.modal-content -->                                                                         ' +
        '  </div><!-- /.modal-dialog -->                                                                                    ' +
        '</div> <!-- /.modal -->                                                                                        ';

    $('body').append(texto);
    $('#' + random).modal('show');
}
