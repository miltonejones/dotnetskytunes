#!/bin/bash

echo "Creating configuration..."
cat > apprunner-config.json << 'EOF'
{
  "ServiceName": "skytunes-app",
  "SourceConfiguration": {
    "AuthenticationConfiguration": {
      "AccessRoleArn": "arn:aws:iam::123823813021:role/AppRunnerServiceRole"
    },
    "ImageRepository": {
      "ImageIdentifier": "123823813021.dkr.ecr.us-east-1.amazonaws.com/skytunescsharp:latest",
      "ImageRepositoryType": "ECR",
      "ImageConfiguration": {
        "Port": "80",
        "RuntimeEnvironmentVariables": {
          "ASPNETCORE_ENVIRONMENT": "Production"
        }
      }
    }
  },
  "InstanceConfiguration": {
    "Cpu": "1024",
    "Memory": "2048"
  }
}
EOF

echo "Deploying to App Runner..."
aws apprunner create-service --cli-input-json file://apprunner-config.json

echo "Monitoring deployment..."
SERVICE_ARN=$(aws apprunner list-services --query "ServiceSummaryList[?ServiceName=='skytunes-app'].ServiceArn" --output text)

while true; do
    STATUS=$(aws apprunner describe-service --service-arn $SERVICE_ARN --query 'Service.Status' --output text)
    URL=$(aws apprunner describe-service --service-arn $SERVICE_ARN --query 'Service.ServiceUrl' --output text)
    
    echo "$(date): Status = $STATUS"
    
    if [ "$STATUS" = "RUNNING" ]; then
        echo "🎉 DEPLOYMENT SUCCESSFUL!"
        echo "Your app: https://$URL"
        break
    elif [ "$STATUS" = "CREATE_FAILED" ]; then
        echo "❌ Failed. Check logs."
        aws apprunner list-operations --service-arn $SERVICE_ARN
        break
    fi
    sleep 30
done


