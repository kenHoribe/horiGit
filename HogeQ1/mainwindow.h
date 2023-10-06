#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QWidget>
#include <QKeyEvent>

QT_BEGIN_NAMESPACE
namespace Ui { class MainWindow; }
QT_END_NAMESPACE

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    MainWindow(QWidget *parent = nullptr);
    ~MainWindow();

private slots:
    void on_pushButton_clicked();
    void on_pushButton_2_clicked();
    void on_pushButton_3_clicked();

    void on_lineEdit_editingFinished();

    void on_pushButton_5_clicked();

private:
    Ui::MainWindow *ui;

    void hogeFunc();
    void Func1();
    void Func2();

protected:
    void keyPressEvent(QKeyEvent *pEvent);

};
#endif // MAINWINDOW_H
