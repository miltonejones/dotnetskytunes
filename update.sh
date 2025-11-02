#!/bin/bash

# Build, push, and deploy
echo "Building new Docker image..."
docker build -t skytunescsharp:latest .

echo "Tagging image..."
docker tag skytunescsharp:latest 123823813021.dkr.ecr.us-east-1.amazonaws.com/skytunescsharp:latest

echo "Pushing to ECR..."
aws ecr get-login-password --region us-east-1 | docker login --username AWS --password-stdin 123823813021.dkr.ecr.us-east-1.amazonaws.com
docker push 123823813021.dkr.ecr.us-east-1.amazonaws.com/skytunescsharp:latest

echo "Starting App Runner deployment..."
SERVICE_ARN=$(aws apprunner list-services --query "ServiceSummaryList[?ServiceName=='skytunescsharp'].ServiceArn" --output text)
aws apprunner start-deployment --service-arn $SERVICE_ARN

echo "Deployment initiated! Check AWS Console for progress."
