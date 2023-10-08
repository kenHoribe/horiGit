#include "replacedialog.h"
#include "ui_replacedialog.h"
#include "mainwindow.h"

ReplaceDialog::ReplaceDialog(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::ReplaceDialog)
{
    ui->setupUi(this);
}

ReplaceDialog::~ReplaceDialog()
{
    delete ui;
}

void ReplaceDialog::on_pushButton_clicked()
{
    emit LotWaferIdUpdated();
}

