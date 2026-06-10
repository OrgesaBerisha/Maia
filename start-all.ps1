$root = "C:\Users\Admin\source\repos\Maia"

$services = @(
    @{ name = "Auth";                path = "$root\Auth\Auth" },
    @{ name = "WomensSection";       path = "$root\WomensSection\WomensSection" },
    @{ name = "MenSection";          path = "$root\MenSection" },
    @{ name = "KidsSection";         path = "$root\KidsSection" },
    @{ name = "OrderService";        path = "$root\OrderService" },
    @{ name = "NotificationService"; path = "$root\NotificationService" },
    @{ name = "ApiGateway";          path = "$root\ApiGateway" }
)

foreach ($svc in $services) {
    Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$($svc.path)'; dotnet run"
}

Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$root\Maia\frontend'; npm run dev"

Write-Host "Te gjitha sherbimet u nisen!" -ForegroundColor Green
