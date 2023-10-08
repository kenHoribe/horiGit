#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include "replacedialog.h"

QT_BEGIN_NAMESPACE
namespace Ui { class MainWindow; }
QT_END_NAMESPACE

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    MainWindow(QWidget *parent = nullptr);
    ~MainWindow();

signals:
    void temporaryEvent();

public slots:
    void FuncA();
    void FuncB();

private slots:
    void on_pushButton_clicked();

private:
    Ui::MainWindow *ui;
    ReplaceDialog* dlg;
};
#endif // MAINWINDOW_H
