require 'rake/clean'
require 'bundler/setup'
require 'albacore'
require 'semver'


desc 'compile Prover.SpecWriter.sln'
build :build do |b|
  b.sln = File.join 'src', 'Prover.SpecWriter-VS2012.sln'
  # b.logging = 'normal'
end

task :default => :build