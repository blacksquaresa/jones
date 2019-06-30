#!/usr/bin/env node

const cli = require("commander");
const { prompt } = require("inquirer");
const jones = require("./package.json");

cli.version(jones.version).description(jones.description);

const initQuestions = [
  {
    type: "input",
    name: "name",
    message: "What is the name of your package?"
  }
];
cli
  .command("init")
  .alias("i")
  .description("Start a new Jones project")
  .action(() => {
    prompt(initQuestions).then(answers => require("./commands/init")(answers));
  });

cli.parse(process.argv);
