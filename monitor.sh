#!/bin/bash

SERVICE_NAME="skytunescsharp"

echo "Monitoring deployment of: $SERVICE_NAME"

while true; do
    # Get service ARN and status
    SERVICE_ARN=$(aws apprunner list-services --query "ServiceSummaryList[?ServiceName=='$SERVICE_NAME'].ServiceArn" --output text)
    
    if [ -z "$SERVICE_ARN" ]; then
        echo "$(date): Service not found yet..."
        sleep 30
        continue
    fi
    
    # Get detailed status
    STATUS=$(aws apprunner describe-service --service-arn $SERVICE_ARN --query 'Service.Status' --output text 2>/dev/null)
    URL=$(aws apprunner describe-service --service-arn $SERVICE_ARN --query 'Service.ServiceUrl' --output text 2>/dev/null)
    
    if [ -z "$STATUS" ]; then
        echo "$(date): Waiting for service to initialize..."
    else
        echo "$(date): Status = $STATUS"
        
        if [ "$STATUS" = "RUNNING" ]; then
            echo "🎉 DEPLOYMENT SUCCESSFUL!"
            echo "Your app is live at: https://$URL"
            echo ""
            echo "Test URLs:"
            echo "https://$URL/"
            echo "https://$URL/Home"
            echo "https://$URL/Home/Index"
            break
        elif [ "$STATUS" = "CREATE_FAILED" ]; then
            echo "❌ Deployment failed. Checking operations..."
            aws apprunner list-operations --service-arn $SERVICE_ARN --query 'OperationSummaryList[0]'
            break
        fi
    fi
    
    sleep 30
done
