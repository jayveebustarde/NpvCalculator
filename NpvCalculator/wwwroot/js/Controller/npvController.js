(function () {
    'use strict';

    angular
        .module('app')
        .controller('NpvController', NpvController);

    NpvController.$inject = ['ApiService'];

    function NpvController(ApiService) {
        var vm = this;

        vm.initialAmount = 0;
        vm.lowerBound = 1.00;
        vm.upperBound = 5.00;
        vm.increment = 0.1;
        vm.cashFlows = [];
        vm.npvList = [];
        vm.lossStyle = { 'color': '#f87066' };
        vm.gainStyle = { 'color': '#66d86c' };
        vm.displayNone = { 'display': 'none' };
        vm.displayTrue = {'display': 'block !important'}
        vm.isComputing = false;

        vm.addCashFlow = addCashFlow;
        vm.compute = compute;
        vm.delCashflow = delCashflow;
        vm.addCashflow = addCashflow;

        function init() {
            for (let i = 0; i < 5; i++)
                vm.cashFlows.push(0);
        }

        init();

        function addCashFlow() {
            vm.cashFlows.push(0);
        }

        function compute() {
            vm.errMessage = null;
            vm.isComputing = true;
            const data = {
                LowerBoundRate: vm.lowerBound,
                UpperBoundRate: vm.upperBound,
                Increment: vm.increment,
                InitialAmount: vm.initialAmount,
                CashFlows: vm.cashFlows
            }

            ApiService.calculateNpv(data)
                .then(function (response) {
                    vm.npvList = response.data;
                    configChart();
                    vm.isComputing = false;
                }, function (err) {
                    vm.errMessage = "Please provide valid input."
                    vm.isComputing = false;
                });
        }

        function configChart() {
            const labels = [];
            const datasets = [{
                label: 'Net Present Value',
                data: [],
                backgroundColor: [],
                borderColor: []
            }];

            for (let net of vm.npvList) {
                labels.push(net.discountRate + '%');
                datasets[0].data.push(net.npv.toFixed(2));
                datasets[0].backgroundColor.push(net.isLoss ? 'rgba(248, 112, 102, 0.2)' : 'rgba(102, 216, 108, 0.2)');
                datasets[0].borderColor.push(net.isLoss ? 'rgba(248, 112, 102)' : 'rgba(102, 216, 108)');

            }
            const data = {
                labels: labels,
                datasets: datasets
            };

            const config = {
                type: 'bar',
                data: data,
                options: {}
            }

            if (vm.npvChart) { vm.npvChart.destroy(); }

            vm.npvChart = new Chart(
                document.getElementById('npvChart'),
                config
            );
        }

        function delCashflow(ix) {
            vm.cashFlows.splice(ix, 1);
        }

        function addCashflow() {
            vm.cashFlows.push(0);
        }
    }
})();