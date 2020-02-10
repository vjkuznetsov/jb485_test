# Тестовое задание
Проверка файлов формата markdown по следующим условиям:
- подсчет суммарного количества изображений и таблиц в файлах;
- есть ли у каждого изображения или таблицы подпись вида "Рисунок", "Таблица";
- результат упакован в docker-контейнер.

Перед созданием контейнера следует скомпилировать проект .NET.

Для создания контейнера:
```docker build . -t mdparser```

Для запуска приложения из контейнера:
```docker run -v D:/data:/data mdparser /data/<filename1>.md /data/<filename2>.md /data/dir/<filename3>.md```

Локальные ресурсы ```D:/data``` будут смонтированы в файловую систему контейнера и доступны по пути ```/data/```

Для сохранения контейнера в виде tar-архива можно использовать команду:
```docker save mdparser > mdparser.tar```

Архив не добавлен в репозиторий, из-за своего размера (185 Мб).




