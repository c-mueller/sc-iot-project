package util

import (
	"github.com/gin-gonic/gin"
	"github.com/google/uuid"
	"github.com/sirupsen/logrus"
)

const (
	GinContextKeyLogger = "logger"
)

func LogEnricherMiddleware(entry *logrus.Entry) gin.HandlerFunc {
	return func(ctx *gin.Context) {
		requestId := uuid.New().String()
		logger := entry.WithField("request_id", requestId)
		ctx.Set(GinContextKeyLogger, logger)
		ctx.Next()
	}
}
