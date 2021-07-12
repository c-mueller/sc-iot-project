package util

import (
	"fmt"
	"github.com/gin-gonic/gin"
	"github.com/sirupsen/logrus"
	"time"
)

func LogMiddleware(log *logrus.Entry, useCtx bool) gin.HandlerFunc {
	return func(c *gin.Context) {
		// Start timer
		start := time.Now()
		path := c.Request.URL.Path
		raw := c.Request.URL.RawQuery

		c.Next()

		end := time.Now()
		latency := end.Sub(start)

		clientIP := c.ClientIP()
		method := c.Request.Method
		statusCode := c.Writer.Status()

		comment := c.Errors.ByType(gin.ErrorTypePrivate).String()

		if raw != "" {
			path = path + "?" + raw
		}

		durMs := float64(latency.Nanoseconds()/1000) / 1000
		message := fmt.Sprintf("[%s]: %s %s - %d: %.3f MS %s", clientIP, method, path, statusCode, durMs, comment)

		messageb := []byte(message)

		logger := log
		if useCtx {
			l, _ := c.Get(GinContextKeyLogger)
			logger = l.(*logrus.Entry)
		}
		logger.WithFields(logrus.Fields{
			"path":        path,
			"client_ip":   clientIP,
			"method":      method,
			"duration":    durMs,
			"status_code": statusCode,
		}).Debug(string(messageb))

	}
}
