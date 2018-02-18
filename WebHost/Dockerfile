FROM rsciriano/docker-mono-apache
ADD ./obj/Release/Package/PackageTmp /var/www/
CMD ["/usr/sbin/apache2ctl", "-D", "FOREGROUND"]
